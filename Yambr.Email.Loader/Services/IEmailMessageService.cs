using System.Threading.Tasks;
using MimeKit;

namespace Yambr.Email.Loader.Services
{
    public interface IEmailMessageService
    {
        Task<bool> MustBeSavedAsync(MimeMessage message);
        Task SaveMessageAsync(MimeMessage message);
    }
}