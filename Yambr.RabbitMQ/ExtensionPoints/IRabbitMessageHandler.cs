using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yambr.SDK.ComponentModel;

namespace Yambr.RabbitMQ.ExtensionPoints
{
    /// <summary>
    /// Точка расширения для обработки сообщений Rabbit
    /// </summary>
    [ExtensionPoint]
    public interface IRabbitMessageHandler
    {
        /// <summary>
        /// Проверить модель
        /// </summary>
        /// <param name="model"> тип сообщения</param>
        /// <returns></returns>
        bool CheckModel(string model);

        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="message">содержимое сообщения</param>
        /// <param name="model"> тип сообщения </param>
        Task ExecuteAsync(string message, string model);
    }
}
