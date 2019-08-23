using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public sealed class ContactRecord
    {
        public ContactRecord(ICollection<Email> emails, ICollection<Phone> phones, ICollection<ContractorSummary> contractors)
        {
            Emails = emails;
            Phones = phones;
            Contractors = contractors;
        }

        public string Fio { get; set; }
        public ICollection<Email> Emails { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ICollection<ContractorSummary> Contractors { get; set; }
    }
}
