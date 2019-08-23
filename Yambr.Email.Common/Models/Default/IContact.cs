using System.Collections.ObjectModel;

namespace Yambr.Email.Common.Models.Default
{
    public interface IContact
    {
        ObservableCollection<Email> Emails { get; set; }
        string Fio { get; set; }
        ObservableCollection<Phone> Phones { get; set; }
        ILocalUser User { get; set; }
    }
}