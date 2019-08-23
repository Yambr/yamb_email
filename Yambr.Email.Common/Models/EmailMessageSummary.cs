using System;

namespace Yambr.Email.Common.Models
{
    public class EmailMessageSummary
    {

        public EmailMessageSummary(MimeMessage message)
        {
            IsBodyHtml = !string.IsNullOrWhiteSpace(message.HtmlBody);
            Body = IsBodyHtml ? message.HtmlBody : message.TextBody;
            DateUtc = message.Date.UtcDateTime;
            Subject = message.Subject;
        }

        public bool IsBodyHtml { get; set; }
        public string Body { get; set; }
        public DateTime DateUtc { get; set; }
        public string Subject { get; set; }
    }
}