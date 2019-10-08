using System.Threading.Tasks;

namespace Yambr.Analyzer.Services
{
    public interface IValueStatsService
    {
        Task UpdateStatsAsync<T>(T contract, string keyParamValue)
            where T : class;
    }
}