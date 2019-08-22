using System;
using System.Collections.Generic;
using System.Text;
using Yambr.Email.SDK.ComponentModel;
using Yambr.Email.SDK.ExtensionPoints;
using Yambr.RabbitMQ.Services;

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
