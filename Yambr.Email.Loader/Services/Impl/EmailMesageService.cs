using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.DistributedCache.Services;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Exceptions;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.Email.Loader.Extensions;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    public class EmailMessageService : IEmailMessageService
    {
        private const string EmailMesageRegion = "EmailMesage";
        private readonly ILogger _logger;
        private readonly ILifetimeScope _lifetimeScope;

        public EmailMessageService(
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
            var emailMessage = //await GetMessageByHashAsync(mailBox, messageHash) ??
                               await CreateMessageAsync(mailBox, message, messageHash);
        }

        #region Сохранение
       /* 
        /// <summary>
        /// Получить сообщение по Хэш
        /// </summary>
        /// <param name="messageHash"></param>
        /// <returns></returns>
        private async Task<EmailMessage> GetMessageByHashAsync(IMailBox mailBox, string messageHash)
        {
            if (string.IsNullOrWhiteSpace(messageHash)) throw new ArgumentNullException(nameof(messageHash));
            var formattableString = MessageKey(mailBox, messageHash);
            return await _cacheService.GetAsync<EmailMessage>(formattableString, EmailMesageRegion);
        }*/

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
                try
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
                catch (EmailLoaderException exception)
                {
                    _logger.Log(LogLevel.Error, exception, $"Ошибка при обработке письма {messageHash}");
                }
            }

         //   var formattableString = MessageKey(mailBox, messageHash);
          //  await _cacheService.InsertAsync(formattableString, emailMessage, EmailMesageRegion, TimeSpan.FromDays(1));
            return emailMessage;
        }

        private static string MessageKey(IMailBox mailBox, string messageHash)
        {
            var formattableString = $"{mailBox.Login}:{messageHash}";
            return formattableString;
        }

        #endregion
    }
}