using System.Collections.Generic;

namespace Yambr.RabbitMQ.Models
{
    /// <summary>
    /// Интерфейс для объекта очереди
    /// </summary>
    public interface IQueueObject
    {
        IDictionary<string, object> Headers { get; set; }
        string RoutingKey { get; set; }
        string Message { get; set; }
     
    }
}