using Newtonsoft.Json;

namespace Yambr.RabbitMQ.Models
{
    /// <summary>
    /// Объект в формате JSON для очереди
    /// </summary>
    public class JsonQueueObject<T> : AbstractQueueObject
    {
        public JsonQueueObject(T messageObject)
        {
            SetMessage(messageObject);
        }

        public JsonQueueObject(T messageObject, string model) : base(model)
        {
            SetMessage(messageObject);
        }
        public JsonQueueObject(
            T messageObject,
            string model, 
            string routingKey) : base(model, routingKey)
        {
            SetMessage(messageObject);
        }

        private void SetMessage(T messageObject)
        {
            Message = JsonConvert.SerializeObject(messageObject);
        }
    }
}