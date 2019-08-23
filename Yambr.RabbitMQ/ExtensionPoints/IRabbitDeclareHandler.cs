using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Yambr.SDK.ComponentModel;

namespace Yambr.RabbitMQ.ExtensionPoints
{
    [ExtensionPoint]
    public interface IRabbitDeclareHandler
    {
        void OnConnect(IModel model);
        IEnumerable<string> ConsumeQueues();
    }
}
