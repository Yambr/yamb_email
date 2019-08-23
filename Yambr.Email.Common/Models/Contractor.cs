using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    
    public class Contractor :  IContractor
    {
       
        /*
        #region Yandex

        public ICollection<Phone> Phones
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


        #endregion*/
        /*
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

        #endregion*/

        public Contractor(ICollection<Domain> domains)
        {
            Domains = domains;
        }

        public string Name { get; set; }
        public ICollection<Domain> Domains { get; set; }
    }
}
