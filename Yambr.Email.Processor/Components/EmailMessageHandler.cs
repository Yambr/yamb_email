using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.RabbitMQ;
using Yambr.RabbitMQ.Models;
using Yambr.RabbitMQ.Services;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Processor.Components
{
    [Component]
    internal class EmailMessageHandler : IEmailMessageHandler
    {
        private readonly ILogger _logger;
        private readonly IRabbitMQService _rabbitMQService;

        public EmailMessageHandler(
            ILogger<EmailMessageHandler> logger,
            IMailBox mailBox,
            IRabbitMQService rabbitMQService)
        {
            MailBox = mailBox;
            _logger = logger;
            _rabbitMQService = rabbitMQService;
        }

        public IMailBox MailBox { get; }

        public Task<EmailMessage> OnCreate(MimeMessage message, EmailMessage emailMessage)
        {
            return Task.FromResult(emailMessage);
        }

        public Task OnSaveAsync(EmailMessage emailMessage)
        {
            _rabbitMQService.SendMessage(
                RabbitMQConstants.EmailExchangeName,
                new JsonQueueObject<EmailMessage>(emailMessage,
                    "EmailMessage",
                    MailBox.User.OwnerQueue ?? string.Empty));

            return Task.CompletedTask;
        }
    }
}
