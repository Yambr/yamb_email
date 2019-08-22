using System;
using System.Collections.Generic;
using System.Text;

namespace Yambr.RabbitMQ
{
    public class RabbitMQSettings
    {
        private const string DefaultModelHeaderKey = "model";
        private const string DefaultAppId = "Yambr.RabbitMQ.Consumer";
        public RabbitMQSettings()
        {
            if (string.IsNullOrWhiteSpace(ModelHeaderKey))
            {
                ModelHeaderKey = DefaultModelHeaderKey;
            }
            if (string.IsNullOrWhiteSpace(AppId))
            {
                AppId = DefaultAppId;
            }
        }
        public string AppId { get; set; }
        public string ModelHeaderKey { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


    }
}
