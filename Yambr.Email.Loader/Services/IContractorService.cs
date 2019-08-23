using System.Threading.Tasks;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services
{
    public interface IContractorService
    {
     /*   void AddDomain(Contractor contractor, string domainString);
        void AddSource(Contractor contractor, string source);
        Task UpdateContractorAddressDataAsync(IContractor contractor);*/
        Task<ContractorSummary> GetOrCreateContractorSummaryByDomainAsync(string domainString);
      /*  Task<Contractor> CreateContractorByDomainAsync(string domainString);
        Task<Contractor> GetContractorByDomainAsync(string domainString);
        Task<Contractor> GetContractorByYandexIdAsync(string id);
        Task<Contractor> GetOrCreateContractorByBusinessModelAsync(Feature businessModel);
        Task<Contractor> GetOrCreateContractorByDomainAsync(string domainString);
        Task UpdateDadataAsync(Contractor contractor);*/
    }
}