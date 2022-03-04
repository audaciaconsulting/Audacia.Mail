namespace Audacia.Mail.Mandrill
{
    public class MandrillOptions
    {
        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string ApiKey { get; set; }

        public bool Async { get; set; }
    }
}
