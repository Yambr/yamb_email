using System.Collections.Generic;
using System.Collections.ObjectModel;
using Yambr.Email.Common.Models.Records.Nested;

namespace Yambr.Email.Common.Models.Default
{
    public interface IContractor: ISynchronizedRecord
    {
       
        string Name { get; set; }

        #region Yandex

        ObservableCollection<Phone> Phones { get; set; }
        List<Category> Categories { get; set; }
        List<Service> Features { get; set; }
        Hours Hours { get; set; }
        string Site { get; set; }
        List<Link> Links { get; set; }
        Geometry Geometry { get; set; }
        /// <summary>
        /// Адрес организации.
        /// </summary>
        string Address { get; set; }

        string YandexId { get; set; }

            #endregion

        #region Dadata

        AddressData AddressData { get; set; }
        string BranchCount { get; set; }
        PartyBranchType? BranchType { get; set; }
        string INN { get; set; }
        string KPP { get; set; }
        PartyManagementData Management { get; set; }
        string OGRN { get; set; }
        string Okpo { get; set; }
        string Okved { get; set; }
        PartyOpfData Opf { get; set; }
        PartyNameData PartyName { get; set; }
        PartyStateData State { get; set; }
        PartyType? Type { get; set; }


        #endregion

        ObservableCollection<Domain> Domains { get; set; }
    }
    
}