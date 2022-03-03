using System.Runtime.Serialization;

namespace Audacia.Mandrill.Models.WebhookJsonDeserialisation
{
    /// <summary>
    /// Represents the type of event for an email to trigger an update from a webhook.
    /// </summary>
    public enum EmailEventType
    {
        /// <summary>There are no recorded events for the email.</summary>
        None = 0,
        /// <summary>The email has been sent from the server.</summary>
        Send = 100,
        /// <summary>The email has been opened by the recipient.</summary>
        Open = 200,
        /// <summary>The email has been hard bounced from the recipient email address.</summary>
        [EnumMember(Value = "hard_bounce")]
        HardBounce = 300,
        /// <summary>The email has been soft bounced from the recipient email address.</summary>
        [EnumMember(Value = "soft_bounce")]
        SoftBounce = 400,
        /// <summary>The email has been marked as spam by the recipient email address.</summary>
        Spam = 500,
        /// <summary>The email has been rejected by the recipient email address.</summary>
        Reject = 600
    }
}