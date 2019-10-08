using System.Threading.Tasks;
using Yambr.Email.Common.Models;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Loader.Services.Impl
{
    /// <summary>
    /// Сервис работы с контрагентами
    /// </summary>
    [Service]
    public class ContractorService : IContractorService
    {
      
        /// <summary>
        /// Получить или создать контрагента по домену
        /// </summary>
        /// <param name="domainString"></param>
        /// <returns></returns>
        public async Task<ContractorSummary> CreateContractorSummaryByDomainAsync(string domainString)
        {

            var contractorSummary = new ContractorSummary()
            {
                Name = domainString
            };


            return contractorSummary;
        }


      
    }
}
