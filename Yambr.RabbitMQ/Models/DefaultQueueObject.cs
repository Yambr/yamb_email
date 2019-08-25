namespace Yambr.RabbitMQ.Models
{
    /// <summary>
    /// Объект по умолчанию для очереди
    /// </summary>
    public class DefaultQueueObject : AbstractQueueObject
    {
        public DefaultQueueObject()
        { }

        public DefaultQueueObject(string model) : base(model)
        { }

        public DefaultQueueObject(string model, string routingKey) : base(model, routingKey)
        { }
        public DefaultQueueObject(string model, string routingKey, string message) : base(model, routingKey, message)
        { }
    }
}