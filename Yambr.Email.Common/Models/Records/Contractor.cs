using System.Collections.Generic;
using System.Collections.ObjectModel;
using Yambr.Email.Common.Models.Default;
using Yambr.Email.Common.Models.Records.Nested;

// ReSharper disable once CheckNamespace
namespace Yambr.Email.Common.Models.Records
{
    [CollectionName(nameof(Contractor))]
    [UploadType(UploadType.SynchronizeAll)]
    public class Contractor : Record, IContractor
    {
        private ObservableCollection<Domain> _domains;
        private string _name;
        private List<Category> _categories;
        private List<Service> _features;
        private Hours _hours;
        private string _address;
        private Geometry _geometry;
        private string _yandexId;
        private ObservableCollection<Phone> _phones;
        private ObservableCollection<DataSource> _sources;
        private AddressData _addressData;
        private string _branchCount;
        private PartyBranchType? _branchType;
        private string _inn;
        private string _kpp;
        private PartyManagementData _management;
        private string _ogrn;
        private string _okpo;
        private string _okved;
        private PartyOpfData _opf;
        private PartyNameData _partyName;
        private PartyStateData _state;
        private PartyType? _type;
        private string _site;
        private List<Link> _links;

        public Contractor()
        {
            Domains = new ObservableCollection<Domain>();
            Phones = new ObservableCollection<Phone>();
            Sources = new ObservableCollection<DataSource>();
            // ReSharper disable once VirtualMemberCallInConstructor
            ClearChangedProperties();
        }


        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        #region Yandex

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

        public List<Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public List<Service> Features
        {
            get => _features;
            set { _features = value; OnPropertyChanged(); }
        }

        public Hours Hours
        {
            get => _hours;
            set { _hours = value; OnPropertyChanged(); }
        }

        public string Site
        {
            get => _site;
            set { _site = value; OnPropertyChanged(); }
        }

        public List<Link> Links
        {
            get => _links;
            set { _links = value;OnPropertyChanged(); }
        }

        public Geometry Geometry
        {
            get => _geometry;
            set { _geometry = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public string YandexId
        {
            get => _yandexId;
            set { _yandexId = value; OnPropertyChanged(); }
        }


        #endregion

        #region Dadata

        public AddressData AddressData
        {
            get => _addressData;
            set { _addressData = value; OnPropertyChanged(); }
        }

        public string BranchCount
        {
            get => _branchCount;
            set { _branchCount = value; OnPropertyChanged(); }
        }

        public PartyBranchType? BranchType
        {
            get => _branchType;
            set { _branchType = value; OnPropertyChanged(); }
        }

        public string INN
        {
            get => _inn;
            set { _inn = value; OnPropertyChanged(); }
        }

        public string KPP
        {
            get => _kpp;
            set { _kpp = value; OnPropertyChanged(); }
        }

        public PartyManagementData Management
        {
            get => _management;
            set { _management = value; OnPropertyChanged(); }
        }

        public string OGRN
        {
            get => _ogrn;
            set { _ogrn = value; OnPropertyChanged(); }
        }

        public string Okpo
        {
            get => _okpo;
            set { _okpo = value; OnPropertyChanged(); }
        }

        public string Okved
        {
            get => _okved;
            set { _okved = value; OnPropertyChanged(); }
        }

        public PartyOpfData Opf
        {
            get => _opf;
            set { _opf = value; OnPropertyChanged(); }
        }

        public PartyNameData PartyName
        {
            get => _partyName;
            set { _partyName = value; OnPropertyChanged(); }
        }

        public PartyStateData State
        {
            get => _state;
            set { _state = value; OnPropertyChanged(); }
        }

        public PartyType? Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        #endregion

        public ObservableCollection<Domain> Domains
        {
            get => _domains;
            set
            {
                _domains = value;
                _domains.CollectionChanged += (sender, args) => OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DataSource> Sources
        {
            get => _sources;
            set
            {
                _sources = value;
                _sources.CollectionChanged += (sender, args) => OnPropertyChanged();
                OnPropertyChanged();
            }
        }
    }
}
