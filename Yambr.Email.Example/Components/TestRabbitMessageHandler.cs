using System;
using Yambr.Email.SDK.ComponentModel;
using Yambr.RabbitMQ.ExtensionPoints;

namespace Yambr.RabbitMQ.Components
{
    [Component]
    public class TestRabbitMessageHandler : IRabbitMessageHandler
    {
        public bool CheckModel(string model)
        {
            Console.WriteLine(model);
            return false;
        }

        public void Execute(string message, string model)
        {
            Console.WriteLine(message);
        }
    }
}
