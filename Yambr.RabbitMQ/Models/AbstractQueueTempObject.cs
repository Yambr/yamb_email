using System;
using System.Collections.Generic;

namespace Yambr.RabbitMQ.Models
{
    /// <summary>
    /// Абстрактный объект для очереди
    /// </summary>
    public abstract class AbstractQueueObject : IQueueObject
    {
        protected AbstractQueueObject()
        {
            Headers = new Dictionary<string, object>();
            RoutingKey = string.Empty;
            Headers.Add("machine_name", Environment.MachineName);
            Headers.Add("timestamp", DateTime.UtcNow.ToString("u"));
        }

        protected AbstractQueueObject(string model) : this()
        {
            //TODO брать из настроек
            Headers.Add("model", model);
        }

        protected AbstractQueueObject(string model, string routingKey) : this(model)
        {
            RoutingKey = routingKey;
        }
        protected AbstractQueueObject(string model, string routingKey, string message) : this(model, routingKey)
        {
            Message = message;
        }

        public IDictionary<string, object> Headers { get; set; }
        public string RoutingKey { get; set; }
        public string Message { get; set; }
    }
}
