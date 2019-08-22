using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Yambr.Email.SDK.ComponentModel;
using Yambr.RabbitMQ;
using Yambr.RabbitMQ.ExtensionPoints;

namespace Yambr.Email.Processor.Components
{
    [Component]
    class ProcessorRabbitDeclareHandler : IRabbitDeclareHandler
    {
        public void OnConnect(IModel model)
        {
            model.ExchangeDeclare(RabbitMQConstants.EmailExchangeName, ExchangeType.Direct, false, false);

            model.QueueDeclare(RabbitMQConstants.TasksQueueName, true, false, false);
            model.QueueBind(
                RabbitMQConstants.TasksQueueName,
                RabbitMQConstants.EmailExchangeName,
                RabbitMQConstants.ToTasksRoutingKey);

            model.QueueDeclare(RabbitMQConstants.EmailQueueName, true, false, false);
            model.QueueBind(
                RabbitMQConstants.EmailQueueName,
                RabbitMQConstants.EmailExchangeName,
                RabbitMQConstants.ToEmailRoutingKey);

            model.QueueDeclare(RabbitMQConstants.PullentiQueueName, true, false, false);
            model.QueueBind(
                RabbitMQConstants.PullentiQueueName,
                RabbitMQConstants.EmailExchangeName,
                RabbitMQConstants.ToPullentiRoutingKey);
        }

        public IEnumerable<string> ConsumeQueues()
        {
            return new[]
            {
                RabbitMQConstants.TasksQueueName,
                RabbitMQConstants.EmailQueueName,
                RabbitMQConstants.PullentiQueueName
            };
        }
    }
}
