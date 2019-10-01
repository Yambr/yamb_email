using System.Collections.Generic;
using RabbitMQ.Client;
using Yambr.RabbitMQ.ExtensionPoints;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Processor.Components
{
    [Component]
    internal class ProcessorRabbitDeclareHandler : IRabbitDeclareHandler
    {
        public void OnConnect(IModel model)
        {
            model.ExchangeDeclare(RabbitMQConstants.ExchangeEmail, ExchangeType.Direct, true, false);

            model.QueueDeclare(RabbitMQConstants.QueueMailboxDownload, true, false, false);
            model.QueueDeclare(RabbitMQConstants.QueueEmailCreated, true, false, false);
            model.QueueBind(
                RabbitMQConstants.QueueEmailCreated,
                RabbitMQConstants.ExchangeEmail,
                RabbitMQConstants.RoutingKeyEmailEventCreated);

            model.QueueDeclare(RabbitMQConstants.QueueMailboxEvents, true, false, false);
            model.QueueBind(
                RabbitMQConstants.QueueMailboxEvents,
                RabbitMQConstants.ExchangeEmail,
                RabbitMQConstants.RoutingKeyMailboxErrorLoading);
            model.QueueBind(
                RabbitMQConstants.QueueMailboxEvents,
                RabbitMQConstants.ExchangeEmail,
                RabbitMQConstants.RoutingKeyMailboxSuccessLoading);
        }

        public IEnumerable<string> ConsumeQueues()
        {
            return new[]
            {
                RabbitMQConstants.QueueMailboxDownload
            };
        }
    }
}
