using System.Collections.ObjectModel;
using Yambr.Email.Common.Models.Entities;
using Yambr.Email.Common.Models.Records.Nested;

// ReSharper disable once CheckNamespace
namespace Yambr.Email.Common.Models.Records
{
    [CollectionName(nameof(Contact))]
    [UploadType(UploadType.SynchronizeAll)]
    public sealed class ContactRecord : Record, ISynchronizedRecord
    {
        private string _fio;
        private ObservableCollection<Email> _emails;
        private ObservableCollection<Phone> _phones;
        private MongoDBRef _user;
        private ObservableCollection<ContractorSummary> _contractors;


        public ContactRecord()
        {
            Contractors = new ObservableCollection<ContractorSummary>();
            Emails = new ObservableCollection<Email>();
            Phones = new ObservableCollection<Phone>();
            ClearChangedProperties();
        }

        public string Fio
        {
            get => _fio;
            set { _fio = value;OnPropertyChanged(); }
        }

        public ObservableCollection<Email> Emails
        {
            get => _emails;
            set
            {
                _emails = value;
                _emails.CollectionChanged += (sender, args) => OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Phone> Phones
        {
            get => _phones;
            set
            {
                _phones = value;
                _phones.CollectionChanged += (sender, args) => OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        public MongoDBRef User
        {
            get => _user;
            set { _user = value;OnPropertyChanged(); }
        }

        public ObservableCollection<ContractorSummary> Contractors
        {
            get => _contractors;
            set
            {
                _contractors = value;
                _contractors.CollectionChanged += (sender, args) => OnPropertyChanged();
                OnPropertyChanged();
            }
        }
    }
}
