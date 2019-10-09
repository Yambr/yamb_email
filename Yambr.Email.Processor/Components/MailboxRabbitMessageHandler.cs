using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Services;
using Yambr.RabbitMQ.Components;
using Yambr.RabbitMQ.Models;
using Yambr.RabbitMQ.Services;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Processor.Components
{
    [Component]
    class MailboxRabbitMessageHandler : AbstractRabbitMessageHandler<MailBox, EmailLoadingStatus>
    {
     
        private readonly ILoaderService _loaderService;
        private readonly ILifetimeScope _lifetimeScope;

        public MailboxRabbitMessageHandler(
            ILogger<MailboxRabbitMessageHandler> logger,
            ILoaderService loaderService,
            ILifetimeScope lifetimeScope
        ) : base(logger)
        {
            _loaderService = loaderService;
            _lifetimeScope = lifetimeScope;
        }

        public override string[] Model => new[] { "Mailbox" };
        public override async Task<EmailLoadingStatus> RunAsync(MailBox mailBox)
        {
            return await _loaderService.LoadFromEmailAsync(mailBox);
        }

        public override Task AfterAsync(MailBox mailBox, EmailLoadingStatus result)
        {
            mailBox.Password = null;
            mailBox.Status = result;
            var rabbitMQService = _lifetimeScope.Resolve<IRabbitMQService>();
            rabbitMQService.SendMessage(
                RabbitMQConstants.ExchangeEmail,
                new JsonQueueObject<MailBox>(mailBox, "Mailbox", RabbitMQConstants.RoutingKeyMailboxSuccessLoading));


            foreach (var contact in mailBox.Contacts)
            {
                rabbitMQService.SendMessage(
                    RabbitMQConstants.ExchangeEmail,
                    new JsonQueueObject<IContact>(contact.Value, "Contact", RabbitMQConstants.RoutingKeyEmailEventCreated
                         ));
            }
            foreach (var contractor in mailBox.Contractors)
            {
                rabbitMQService.SendMessage(
                    RabbitMQConstants.ExchangeEmail,
                    new JsonQueueObject<IContractor>(contractor.Value, "Contractor", RabbitMQConstants.RoutingKeyEmailEventCreated
                          ));
            }
            return base.AfterAsync(mailBox, result);
        }

        public override Task ErrorCallBack(MailBox mailBox, Exception exception)
        {
            mailBox.Password = null;
            mailBox.Status = EmailLoadingStatus.Error;
            mailBox.Error = $"Ошибка загрузки из ящика: {exception.Message}\n{exception.StackTrace}";
            var rabbitMQService = _lifetimeScope.Resolve<IRabbitMQService>();
            rabbitMQService.SendMessage(
                RabbitMQConstants.ExchangeEmail,
                new JsonQueueObject<MailBox>(mailBox, "Mailbox", RabbitMQConstants.RoutingKeyMailboxErrorLoading));
            return base.ErrorCallBack(mailBox, exception);
        }
    }
}
