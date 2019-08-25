using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.Email.Loader.Extensions;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    public class EmailMesageService : IEmailMessageService
    {
        private const string EmailMesageRegion = "EmailMesage";
        private readonly ILogger _logger;
        private readonly ILifetimeScope _lifetimeScope;

        public EmailMesageService(
            ILogger<IEmailMessageService> logger,
            ILifetimeScope lifetimeScope)
        {
            _logger = logger;
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// можно ли сохранить 
        /// </summary>
        /// <param name="mailBox"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<bool> MustBeSavedAsync(IMailBox mailBox, MimeMessage message)
        {
            return Task.FromResult(message.Date.UtcDateTime > mailBox.LastStartTimeUtc);
        }

        /// <summary>
        /// Сохранить сообщение
        /// </summary>
        /// <param name="mailBox"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SaveMessageAsync(IMailBox mailBox, MimeMessage message)
        {
            var messageHash = message.MessageHash();
            _logger.Info($"Сообщение от {message.Date} хэш {messageHash}");
            var emailMessage = await GetMessageByHashAsync(messageHash) ??
                               await CreateMessageAsync(mailBox, message, messageHash);
        }

        #region Сохранение
        
        /// <summary>
        /// Получить сообщение по Хэш
        /// </summary>
        /// <param name="messageHash"></param>
        /// <returns></returns>
        private async Task<EmailMessage> GetMessageByHashAsync(string messageHash)
        {
            if (string.IsNullOrWhiteSpace(messageHash)) throw new ArgumentNullException(nameof(messageHash));
           //TODO кеш
           return null;
        }

        /// <summary>
        /// создать сообщения (без сохранения в бд)
        /// </summary>
        /// <param name="mailBox"></param>
        /// <param name="message"></param>
        /// <param name="messageHash"></param>
        /// <returns></returns>
        private async Task<EmailMessage> CreateMessageAsync(IMailBox mailBox, MimeMessage message, string messageHash)
        {
            //заполним основные поля
            var emailMessage = new EmailMessage
            {
                Subject = message.Subject,
                DateUtc = message.Date.UtcDateTime,
                Hash = messageHash
            };
            _logger.Info($"Создано сообщение {messageHash}");

            using (var scope = _lifetimeScope.BeginLifetimeScope((builder) =>
            {
                builder.Register(c => mailBox).As<IMailBox>();
            }))
            {
                var messageHandlers = scope.Resolve<IEnumerable<IEmailMessageHandler>>();
                var emailMessageHandlers = messageHandlers as IEmailMessageHandler[] ?? messageHandlers.ToArray();
                foreach (var emailMessageHandler in emailMessageHandlers)
                {
                    await emailMessageHandler.OnCreate(message, emailMessage);
                }
                foreach (var emailMessageHandler in emailMessageHandlers)
                {
                    await emailMessageHandler.OnSaveAsync(emailMessage);
                }
            }
            return emailMessage;
        }


        #endregion
    }
}