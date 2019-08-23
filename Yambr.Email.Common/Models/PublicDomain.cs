namespace Yambr.Email.Common.Models
{
    /// <summary>
    /// Публичный домен (хранится в основной БД используется для исключения из доменов создающих компании)
    /// </summary>
    public class PublicDomain :  IDomain
    {
        public string DomainString { get; set; }
    }
}
