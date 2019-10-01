using System;
using System.Threading.Tasks;
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
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILoaderService _loaderService;

        public MailboxRabbitMessageHandler(
            ILogger<MailboxRabbitMessageHandler> logger,
            IRabbitMQService rabbitMQService,
            ILoaderService loaderService
        ) : base(logger)
        {
            _rabbitMQService = rabbitMQService;
            _loaderService = loaderService;
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
            _rabbitMQService.SendMessage(
                RabbitMQConstants.ExchangeEmail,
                new JsonQueueObject<MailBox>(mailBox, "Mailbox", RabbitMQConstants.RoutingKeyMailboxSuccessLoading));
            return base.AfterAsync(mailBox, result);
        }

        public override Task ErrorCallBack(MailBox mailBox, Exception exception)
        {
            mailBox.Password = null;
            mailBox.Status = EmailLoadingStatus.Error;
            mailBox.Error = $"Ошибка загрузки из ящика: {exception.Message}\n{exception.StackTrace}";
            _rabbitMQService.SendMessage(
                RabbitMQConstants.ExchangeEmail,
                new JsonQueueObject<MailBox>(mailBox, "Mailbox", RabbitMQConstants.RoutingKeyMailboxErrorLoading));
            return base.ErrorCallBack(mailBox, exception);
        }
    }
}
