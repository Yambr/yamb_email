using System;

namespace Yambr.RabbitMQ.Exceptions
{
    /// <summary>
    /// Ошибка в работе Rabbit
    /// </summary>
    public class RabbitException : Exception
    {
        public RabbitException(string message) : base(message)
        { }

        public RabbitException(string message, Exception innerException):base(message, innerException)
        { }
    }
}
