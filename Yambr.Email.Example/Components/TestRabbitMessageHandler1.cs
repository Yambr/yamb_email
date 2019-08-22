using System;
using Yambr.Email.SDK.ComponentModel;
using Yambr.RabbitMQ.ExtensionPoints;

namespace Yambr.Email.Example.Components
{
    [Component]
    public class TestRabbitMessageHandler1 : IRabbitMessageHandler
    {
        public bool CheckModel(string model)
        {
            Console.WriteLine(model+"12");
            return false;
        }

        public void Execute(string message, string model)
        {
            Console.WriteLine(message + "12");
        }
    }
}
