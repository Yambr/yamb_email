using System;
using System.Collections.Generic;
using System.Text;
using Yambr.RabbitMQ.Services;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.RabbitMQ.Components
{
    [Component]
    public class RabbitMQInitHandler : IInitHandler
    {
        private readonly IRabbitMQService _rabbitMQService;

        public RabbitMQInitHandler(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }
        public void Init()
        {
            
        }

        public void InitComplete()
        {
            _rabbitMQService.Init();
        }
    }
}
