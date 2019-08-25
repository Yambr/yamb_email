using System.Threading.Tasks;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services
{
    public interface ILoaderService
    {
        Task<EmailLoadingStatus> LoadFromEmailAsync(IMailBox mailBox);
    }
}