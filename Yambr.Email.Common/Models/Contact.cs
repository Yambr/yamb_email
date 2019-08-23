using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public class Contact : IContact
    {
        public ICollection<Email> Emails { get; set; }
        public string Fio { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public ILocalUser User { get; set; }

        public ICollection<ContractorSummary> Contractors { get; set; }
}
}
