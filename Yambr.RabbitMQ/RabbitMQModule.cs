using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Yambr.SDK.Autofac;

namespace Yambr.RabbitMQ
{
    public class RabbitMQModule : AbstractModule
    {
        const string RabbitMQSettingsName = nameof(RabbitMQSettings);
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(c =>
                {
                    var configurationRoot = c.Resolve<IConfiguration>();
                    var rabbitMQSettings = new RabbitMQSettings();
                    configurationRoot.Bind(RabbitMQSettingsName, rabbitMQSettings);
                    return rabbitMQSettings;
                }).SingleInstance();
            base.Load(containerBuilder);
        }
    }
}
