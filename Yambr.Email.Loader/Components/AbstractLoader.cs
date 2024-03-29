﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.Email.Common.EncryptionHelper;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.Email.Loader.Services;

namespace Yambr.Email.Loader.Components
{
    public abstract class AbstractLoader : IEmailLoader
    {
        protected readonly ILogger Logger;
        private readonly IEmailMessageService _emailMesageService;

        protected AbstractLoader(
            ILogger logger,
            IEmailMessageService emailMesageService)
        {
            Logger = logger;
            _emailMesageService = emailMesageService;
        }

        /// <summary>
        /// Проверка нужно ли сохранять сообщение из почты 
        /// (проверяет нет ли такого же письма в бд и даты что подходят под рамки)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task<bool> MustBeSavedAsync(IMailBox mailBox, MimeMessage message)
        {
            return await _emailMesageService.MustBeSavedAsync(mailBox, message);
        }

        /// <summary>
        /// Полное сохранение письма 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SaveMessageAsync(IMailBox mailBox, MimeMessage message)
        {

          
            
            await _emailMesageService.SaveMessageAsync(mailBox, message);
        }

        protected async Task AuthorizeAsync(IMailService client, IMailBox mailBox)
        {
            try
            {
                //TODO
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                var password = StringCipher.Decrypt(mailBox.Password, mailBox.Login);
                await client.AuthenticateAsync(mailBox.Login, password);
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось авторизоваться {mailBox.Login} - {ex.Message}",ex);
            }
        }

        protected async Task ConnectAsync(IMailService client, IServer server)
        {
            try
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(server.Host, server.Port, server.UseSsl);
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось подключиться к {server.Host} - {ex.Message}",ex);
            }
        }

        public abstract ConnectionType ConnectionType { get; }
        public abstract Task<EmailLoadingStatus> LoadFromEmailAsync(IMailBox mailBox);
    }
}