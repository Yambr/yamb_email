using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Services;
using Yambr.RabbitMQ.Components;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Processor.Components
{
    [Component]
    class MailboxMessageHandler : AbstractRabbitMessageHandler<MailBox, EmailLoadingStatus>
    {
        private readonly ILoaderService _loaderService;

        public MailboxMessageHandler(
            ILogger<MailboxMessageHandler> logger,
            ILoaderService loaderService
        ) : base(logger)
        {
            _loaderService = loaderService;
        }

        public override string[] Model => new[] {"Mailbox"};
        public override async Task<EmailLoadingStatus> RunAsync(MailBox mailBox)
        {
            return await _loaderService.LoadFromEmailAsync(mailBox);
        }
    }
}
