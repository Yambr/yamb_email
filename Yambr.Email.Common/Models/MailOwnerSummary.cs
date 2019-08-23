using System;

namespace Yambr.Email.Common.Models
{
    /// <summary>
    ///  Владелец пиьсма
    /// </summary>
    public class MailOwnerSummary : IEquatable<MailOwnerSummary>
    {
        public MailOwnerSummary()
        { }

        public MailOwnerSummary(string email, string fio = "")
        {
            Email = email;
            Fio = fio;
        }

        public string Email { get; set; }
        public string Fio { get; set; }

        public bool Equals(MailOwnerSummary other)
        {
            return other != null && Email == other.Email;
        }
    }
}
