using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yambr.Email.Common.Models
{
    public class Contact : IContact
    {
        [JsonConstructor]
        public Contact(List<Email> emails, List<Phone> phones, LocalUser user, List<ContractorSummary> contractors)
        {
            Emails = emails;
            Phones = phones;
            User = user;
            Contractors = contractors;
        }

        public Contact()
        {
        }

        public ICollection<Email> Emails { get; set; }
        public string Fio { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ILocalUser User { get; set; }

        public ICollection<ContractorSummary> Contractors { get; set; }
}
}
