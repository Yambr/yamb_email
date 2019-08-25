using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Logging;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Services;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;


namespace Yambr.Email.Loader.Components
{
    [Component]
    public class ImapLoader : AbstractLoader
    {
        public ImapLoader(
            ILogger<ImapLoader> logger,
            IEmailMessageService emailMesageService
            ) : base(logger, emailMesageService)
        {
        }

        public override ConnectionType ConnectionType => ConnectionType.IMAP;

        public override async Task<EmailLoadingStatus> LoadFromEmailAsync(IMailBox mailBox)
        {
            using (var client = new ImapClient())
            {
                await ConnectAsync(client, mailBox.Server);
                await AuthorizeAsync(client, mailBox);

                Logger.Info($"Подключены к {mailBox.Login} c помощью {nameof(ImapLoader)}");

                // The Inbox folder is always available on all IMAP servers...
                await  LoadFromFolderAsync(mailBox, client.Inbox.ParentFolder);

                await client.DisconnectAsync(true);
            }

            return EmailLoadingStatus.Active;
        }

        private async Task LoadFromFolderAsync(IMailBox mailBox, IMailFolder parentFolder)
        {
            var folders = await parentFolder.GetSubfoldersAsync();

            using (var folderEnumerator = folders.GetEnumerator())
            {
                while (folderEnumerator.MoveNext())
                {
                    if (MustBeOpen(folderEnumerator.Current))
                    {
                        var folder = folderEnumerator.Current;
                        await folder.OpenAsync(FolderAccess.ReadOnly);
                        if (folder.Count > 0)
                        {
                            for (var i = folder.Count-1; i > 0; i--)
                            {
                                
                                var message = await folder.GetMessageAsync(i);
                                // только такая проверка т.к. в POP3 без вариантов
                                if (await MustBeSavedAsync(mailBox, message))
                                {
                                    await SaveMessageAsync(mailBox, message);
                                    continue;
                                }
                                //если письмо сохранять не нужно то сразу выходим
                                break;
                            }

                        }
                        if ((folderEnumerator.Current.Attributes & FolderAttributes.HasChildren) != 0)
                        {
                            await LoadFromFolderAsync(mailBox, folderEnumerator.Current);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ПРоверяет нужно ли открыть папку
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool MustBeOpen(IMailFolder folder)
        {
            return !((folder.Attributes & FolderAttributes.Trash) != 0 ||
                     (folder.Attributes & FolderAttributes.Archive) != 0 ||
                     (folder.Attributes & FolderAttributes.Junk) != 0 ||
                     (folder.Attributes & FolderAttributes.Drafts) != 0 ||
                     (folder.Attributes & FolderAttributes.NonExistent) != 0 ||
                     (folder.Attributes & FolderAttributes.Archive) != 0);
        }
    }
}
