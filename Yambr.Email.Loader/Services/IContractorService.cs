using System.Threading.Tasks;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services
{
    public interface IContractorService
    {
        Task<ContractorSummary> CreateContractorSummaryByDomainAsync(string domainString);
    }
}