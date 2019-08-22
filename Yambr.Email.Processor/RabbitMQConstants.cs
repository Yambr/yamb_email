using System;
using System.Collections.Generic;
using System.Text;

namespace Yambr.RabbitMQ
{
    public static class RabbitMQConstants
    {
        public const string ToTasksRoutingKey = "ToTasksQueueYambr";
        public const string TasksQueueName = "TasksQueueYambr";

        public const string ToEmailRoutingKey = "ToEmailQueueYambr";
        public const string EmailQueueName = "EmailQueueYambr";

        public const string ToPullentiRoutingKey = "ToPullentiQueueYambr";
        public const string PullentiQueueName = "PullentiQueueYambr";

        public const string EmailExchangeName = "EmailExchangeYambr";
    }
}
