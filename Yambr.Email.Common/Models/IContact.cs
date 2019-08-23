using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface IContact
    {
        ICollection<Email> Emails { get; set; }
        string Fio { get; set; }
        ICollection<Phone> Phones { get; set; }
        ILocalUser User { get; set; }
    }
}