using System;
using Yambr.Email.Common.Models.Entities;
using Yambr.Email.Common.Models.Records;

namespace Yambr.Email.Common.Models
{
    /// <summary>
    /// Сокращенный вид для храненя в письме
    /// </summary>
    public class ContactSummary : ISummary
    {
        [BsonIgnore]
        public readonly ContactRecord Contact;

        public ContactSummary()
        {}

        public ContactSummary(string normalizedEmail, [NotNull] ContactRecord contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            Email = normalizedEmail;
            Fio = contact.Fio;
            Ref = contact.CreateDBRef();
            Contact = contact;
        }
        public ContactSummary(string normalizedEmail, [NotNull] Contact contact):this(normalizedEmail,contact.Record)
        {}

        public string Fio { get; set; }

        public string Email { get; set; }

        public MongoDBRef Ref { get; set; }
    }
}
