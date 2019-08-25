using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    class LoaderService : ILoaderService
    {
        private readonly IEnumerable<IEmailLoader> _emailLoaders;

        public LoaderService(IEnumerable<IEmailLoader> emailLoaders)
        {
            _emailLoaders = emailLoaders;
        }

        public async Task<EmailLoadingStatus> LoadFromEmailAsync(IMailBox mailBox)
        {
            foreach (var emailLoader in _emailLoaders.Where(c=>c.ConnectionType == mailBox.Server.ConnectionType))
            {
                return await emailLoader.LoadFromEmailAsync(mailBox);
            }

            return EmailLoadingStatus.Error;
        }
    }
}
