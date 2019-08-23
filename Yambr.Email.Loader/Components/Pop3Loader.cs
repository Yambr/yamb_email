using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Pop3;
using Microsoft.Extensions.Logging;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.Email.Loader.Services;
using Yambr.Email.Loader.Services.Default;
using Yambr.Email.SDK.ComponentModel;
using Yambr.Email.SDK.Extensions;

namespace Yambr.Email.Loader.Components
{
    [Component]
    public class Pop3Loader : AbstractLoader, IEmailLoader
    {
        public Pop3Loader(
            ILogger<Pop3Loader> logger, 
            IEmailMessageService emailMesageService
            ) : base(logger,  emailMesageService)
        {
        }

        public override ConnectionType ConnectionType => ConnectionType.POP3;

        public override async Task<EmailLoadingStatus> LoadFromEmailAsync(IMailBox mailBox)
        {
            using (var client = new Pop3Client())
            {
                await ConnectAsync(client, mailBox.Server);
                await AuthorizeAsync(client, mailBox);

                Logger.Info($"Подключены к {mailBox.Login} c помощью {nameof(ImapLoader)}");
                await LoadMessagesAsync(client);

                await client.DisconnectAsync(true, CancellationToken.None);
            }
            return EmailLoadingStatus.Active;
        }

        /// <summary>
        /// Загрузим сообщения
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task LoadMessagesAsync(Pop3Client client)
        {
            var count = client.Count;
            for (var i = count-1; i > 0; i--)
            {
               var message = await client.GetMessageAsync(i);
               
                // только такая проверка т.к. в POP3 без вариантов
                if (await MustBeSavedAsync(message))
                {
                    await SaveMessageAsync(message);
                    continue;
                }
                //если письмо сохранять не нужно то сразу выходим
                break;
            }
        }

    }
}
