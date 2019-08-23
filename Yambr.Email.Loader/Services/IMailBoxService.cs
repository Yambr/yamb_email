using System.Threading.Tasks;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services
{
    public interface IMailBoxService
    {
        Task<MailBox> GetMailBoxByEmail(string email);
    }
}