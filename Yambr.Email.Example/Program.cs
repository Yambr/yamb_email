using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Yambr.DistributedCache.Services;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.RabbitMQ.ExtensionPoints;
using Yambr.SDK.Autofac;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.Email.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            // The Microsoft.Extensions.DependencyInjection.ServiceCollection
            // has extension methods provided by other .NET Core libraries to
            // regsiter services with DI.
            var serviceCollection = new ServiceCollection();

            var serviceProvider = ConfigureServices(serviceCollection, configuration);

            var initHandlers = serviceProvider.GetServices<IInitHandler>();
            foreach (var initHandler in initHandlers)
            {
                initHandler.InitComplete();
            }

            Console.WriteLine("Hello World!");
        }

        private static AutofacServiceProvider ConfigureServices(ServiceCollection serviceCollection,
            IConfigurationRoot configuration)
        {
            serviceCollection
                .AddLogging(opt =>
                {
                    opt.AddConsole();
                    opt.AddConfiguration(configuration.GetSection("Logging"));
                });
            serviceCollection.AddStackExchangeRedisCache(options =>
            {
                var section = configuration.GetSection(nameof(RedisCache));
                section.Bind(options);
            });
            
            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .Register(c => configuration)
                .AsImplementedInterfaces()
                .SingleInstance();

            // Once you've registered everything in the ServiceCollection, call
            // Populate to bring those registrations into Autofac. This is
            // just like a foreach over the list of things in the collection
            // to add them to Autofac.
            containerBuilder.Populate(serviceCollection);
            containerBuilder.ReagisterAllModules();

            // Creating a new AutofacServiceProvider makes the container
            // available to your app using the Microsoft IServiceProvider
            // interface so you can use those abstractions rather than
            // binding directly to Autofac.
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);
            return serviceProvider;
        }
    }
}
