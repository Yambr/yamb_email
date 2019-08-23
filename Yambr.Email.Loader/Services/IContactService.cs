using System.Threading.Tasks;
using MimeKit;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services
{
    public interface IContactService 
    {
        Task<ContactSummary> GetOrCreateContactSummaryAsync(MailboxAddress mailbox);
    }
}