using System;

namespace Yambr.Email.Common.Models
{
    public class EmailMessageSummary
    {
        public bool IsBodyHtml { get; set; }
        public string Body { get; set; }
        public DateTime DateUtc { get; set; }
        public string Subject { get; set; }
    }
}