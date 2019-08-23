using Yambr.Email.Common.Models.Default;

namespace Yambr.Email.Common.Models.Records
{
    /// <summary>
    /// Публичный домен (хранится в основной БД используется для исключения из доменов создающих компании)
    /// </summary>
    [CollectionName(nameof(PublicDomain))]
    public class PublicDomain : Record, IDomain
    {
        private string _domain;

        public string DomainString
        {
            get => _domain;
            set { _domain = value; OnPropertyChanged(); }
        }
    }
}
