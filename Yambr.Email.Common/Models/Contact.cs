using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yambr.Email.Common.Models
{
    public class Contact : IContact
    {
        [JsonConstructor]
        public Contact(List<Email> emails, List<Phone> phones, LocalUser user, Contractor contractor) :this()
        {
            Emails = emails;
            Phones = phones;
            User = user;
            Contractor = contractor;
        }

        public Contact()
        {
            Emails = new List<Email>();
            Phones = new List<Phone>();
        }

        public ICollection<Email> Emails { get; set; }
        public string Fio { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ILocalUser User { get; set; }

        public IContractor Contractor { get; set; }

        public string Gender { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
    }
}
