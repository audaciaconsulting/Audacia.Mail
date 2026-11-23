using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Audacia.Mail.Tests.MailKit;

/// <summary>
/// A minimal SMTP server that listens on the loopback interface only and records every message it receives.
/// It never relays mail anywhere, so tests using it cannot send real email.
/// </summary>
internal sealed class FakeSmtpServer : IAsyncDisposable
{
    private readonly TcpListener _listener;
    private readonly CancellationTokenSource _cancellation = new();
    private readonly ConcurrentQueue<ReceivedSmtpMessage> _messages = new();
    private readonly ConcurrentBag<Task> _sessions = [];
    private readonly bool _advertiseAuthentication;
    private readonly Task _acceptLoop;
    private int _connectionCount;

    /// <summary>Initializes a new instance of the <see cref="FakeSmtpServer"/> class and starts listening on a free loopback port.</summary>
    /// <param name="advertiseAuthentication">Whether the server should advertise and require AUTH PLAIN.</param>
    public FakeSmtpServer(bool advertiseAuthentication = false)
    {
        _advertiseAuthentication = advertiseAuthentication;
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
        _acceptLoop = AcceptLoopAsync();
    }

    /// <summary>Gets the host the server listens on. Always the loopback address.</summary>
    public static string Host => IPAddress.Loopback.ToString();

    /// <summary>Gets the port the server listens on.</summary>
    public int Port { get; }

    /// <summary>Gets the messages received so far, in the order they arrived.</summary>
    public IReadOnlyList<ReceivedSmtpMessage> Messages => [.. _messages];

    /// <summary>Gets the number of TCP connections the server has accepted.</summary>
    public int ConnectionCount => Volatile.Read(ref _connectionCount);

    /// <summary>Gets the credentials supplied via AUTH PLAIN, if any.</summary>
    public NetworkCredential? ReceivedCredentials { get; private set; }

    /// <summary>Creates <see cref="SmtpSettings"/> that point at this server.</summary>
    /// <param name="userName">The user name to authenticate with.</param>
    /// <param name="password">The password to authenticate with.</param>
    /// <param name="defaultSender">The default sender address.</param>
    /// <returns>The settings.</returns>
    public SmtpSettings CreateSettings(string userName = "", string password = "", string defaultSender = "")
    {
        return new SmtpSettings
        {
            Host = Host,
            Port = Port,
            UserName = userName,
            Password = password,
            DefaultSender = defaultSender
        };
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _cancellation.CancelAsync();
        _listener.Stop();
        await IgnoreShutdownErrorsAsync(_acceptLoop);
        foreach (var session in _sessions)
        {
            await IgnoreShutdownErrorsAsync(session);
        }

        _listener.Dispose();
        _cancellation.Dispose();
    }

    private static async Task IgnoreShutdownErrorsAsync(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception exception) when (exception is OperationCanceledException or SocketException or IOException or ObjectDisposedException)
        {
            // Expected while shutting down.
        }
    }

    private async Task AcceptLoopAsync()
    {
        while (!_cancellation.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync(_cancellation.Token);
            Interlocked.Increment(ref _connectionCount);
            _sessions.Add(RunSessionAsync(client));
        }
    }

    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Ownership of the accepted client is transferred to the session.")]
    private async Task RunSessionAsync(TcpClient client)
    {
        using (client)
        {
            var stream = client.GetStream();
            await using (stream)
            {
                using var reader = new StreamReader(stream, Encoding.Latin1, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                await using var writer = new StreamWriter(stream, Encoding.Latin1, leaveOpen: true) { NewLine = "\r\n", AutoFlush = true };
                await HandleCommandsAsync(reader, writer);
            }
        }
    }

    private async Task HandleCommandsAsync(StreamReader reader, StreamWriter writer)
    {
        var token = _cancellation.Token;
        await writer.WriteLineAsync("220 localhost Fake SMTP server ready");

        var mailFrom = string.Empty;
        var recipients = new List<string>();

        while (await reader.ReadLineAsync(token) is { } line)
        {
            var command = line.Length >= 4 ? line[..4].ToUpperInvariant() : line.ToUpperInvariant();
            switch (command)
            {
                case "EHLO":
                    await WriteEhloResponseAsync(writer);
                    break;
                case "HELO":
                    await writer.WriteLineAsync("250 localhost");
                    break;
                case "AUTH":
                    await AuthenticateAsync(line, reader, writer, token);
                    break;
                case "MAIL":
                    mailFrom = ExtractAddress(line);
                    recipients = [];
                    await writer.WriteLineAsync("250 2.1.0 OK");
                    break;
                case "RCPT":
                    recipients.Add(ExtractAddress(line));
                    await writer.WriteLineAsync("250 2.1.5 OK");
                    break;
                case "DATA":
                    await writer.WriteLineAsync("354 Start mail input; end with <CRLF>.<CRLF>");
                    var data = await ReadDataAsync(reader, token);
                    _messages.Enqueue(new ReceivedSmtpMessage(mailFrom, recipients, data));
                    await writer.WriteLineAsync("250 2.0.0 OK: queued");
                    break;
                case "QUIT":
                    await writer.WriteLineAsync("221 2.0.0 Bye");
                    return;
                default:
                    await writer.WriteLineAsync("250 OK");
                    break;
            }
        }
    }

    private async Task WriteEhloResponseAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync("250-localhost");
        if (_advertiseAuthentication)
        {
            await writer.WriteLineAsync("250-AUTH PLAIN");
        }

        await writer.WriteLineAsync("250 HELP");
    }

    private async Task AuthenticateAsync(string line, StreamReader reader, StreamWriter writer, CancellationToken token)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var initialResponse = parts.Length > 2 ? parts[2] : null;
        if (initialResponse == null)
        {
            await writer.WriteLineAsync("334 ");
            initialResponse = await reader.ReadLineAsync(token) ?? string.Empty;
        }

        // AUTH PLAIN payload is base64("authzid\0authcid\0password").
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(initialResponse)).Split('\0');
        ReceivedCredentials = new NetworkCredential(decoded[1], decoded[2]);
        await writer.WriteLineAsync("235 2.7.0 Authentication successful");
    }

    private static async Task<string> ReadDataAsync(StreamReader reader, CancellationToken token)
    {
        var builder = new StringBuilder();
        while (await reader.ReadLineAsync(token) is { } line && line != ".")
        {
            // Undo SMTP dot-stuffing.
            builder.Append(line.StartsWith("..", StringComparison.Ordinal) ? line[1..] : line).Append("\r\n");
        }

        return builder.ToString();
    }

    private static string ExtractAddress(string line)
    {
        var start = line.IndexOf('<', StringComparison.Ordinal);
        var end = line.IndexOf('>', StringComparison.Ordinal);
        return start >= 0 && end > start ? line.Substring(start + 1, end - start - 1) : string.Empty;
    }
}
