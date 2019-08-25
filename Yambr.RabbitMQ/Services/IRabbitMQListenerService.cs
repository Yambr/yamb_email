using RabbitMQ.Client.Events;
using Yambr.RabbitMQ.Models;

namespace Yambr.RabbitMQ.Services
{
    /// <summary>
    /// Сервис работы с очередью Rabbit
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IRabbitMQService
    {
        /// <summary>
        /// Подключиться к Очереди
        /// </summary>
        void Init();
        /// <summary>
        /// Закрыть подключения
        /// </summary>
        void DisposeConnection();

        /// <summary>
        /// Получить модель сообщения (тип сообщения)
        /// </summary>
        /// <param name="eventArgs"> Contains all the information about a message delivered
        /// from an AMQP broker within the Basic content-class.
        /// (содержит информацию о сообщении, в заголовках можно указать дополнительные параметры)
        /// </param>
        /// <param name="message">сообщение</param>
        /// <returns></returns>
        string GetModelFromMessage(BasicDeliverEventArgs eventArgs, string message);

        /// <summary>
        /// Отправить сообщение немедленно
        /// в exchange очереди
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueObject">сообщение для отправки</param>
        void SendMessage(string exchangeName, IQueueObject queueObject);

    }
}
