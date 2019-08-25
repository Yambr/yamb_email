using System.Threading.Tasks;
using MimeKit;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services
{
    public interface IEmailMessageService
    {
        Task<bool> MustBeSavedAsync(IMailBox mailBox, MimeMessage message);
        Task SaveMessageAsync(IMailBox mailBox, MimeMessage message);
    }
}