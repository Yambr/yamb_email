using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services.Impl
{
    public class MailBoxService : IMailBoxService
    {
       
        private readonly ILogger _logger;

        public MailBoxService(
            ILogger<MailBoxService> logger)
        {
            _logger = logger;
        }

        public async Task<MailBox> GetMailBoxByEmail(string email)
        {
            /*  TODO Cache
            var filterDefinition = new FilterDefinitionBuilder<MailBoxRecord>().Eq(c => c.Login, email);
            var cursor = await _mailBoxRecordCollection.FindAsync(filterDefinition,
                new FindOptions<MailBoxRecord>
                {
                    Limit = 1
                });

            while (cursor.MoveNext())
            {
                var record = cursor.Current.FirstOrDefault();
                if (record != null)
                {
                    return new MailBox(_scope, record);
                }
            }*/
            return null;
        }
        
    }
}
