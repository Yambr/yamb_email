using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Yambr.Email.Example.ExtensionPonts;
using Yambr.Email.Example.Services;
using Yambr.Email.Example.Services.Impl;
using Yambr.Email.Processor;
using Yambr.Email.SDK;
using Yambr.Email.SDK.Autofac;
using Yambr.Email.SDK.ExtensionPoints;
using Yambr.RabbitMQ;
using Yambr.RabbitMQ.ExtensionPoints;

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

            // The Microsoft.Extensions.Logging package provides this one-liner
            // to add logging services.
            serviceCollection.AddLogging();

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
            ProcessorModule е = null;
            containerBuilder.ReagisterAllModules();
            
                // Creating a new AutofacServiceProvider makes the container
                // available to your app using the Microsoft IServiceProvider
                // interface so you can use those abstractions rather than
                // binding directly to Autofac.
                var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            var initHandlers = serviceProvider.GetServices<IInitHandler>();
            foreach (var initHandler in initHandlers)
            {
                initHandler.InitComplete();
            }

            var rabbitMessageHandlers = serviceProvider.GetServices<IRabbitMessageHandler>();
            foreach (var rabbitMessageHandler in rabbitMessageHandlers)
            {
                rabbitMessageHandler.CheckModel("rabbitMessageHandler");
                rabbitMessageHandler.Execute("rabbitMessageHandler", "");
            }
            var testExtensionPoints = serviceProvider.GetServices<ITestExtensionPoint>();
            foreach (var testExtensionPoint in testExtensionPoints)
            {
                testExtensionPoint.Test("testExtensionPoint");
            }
           
           
            var service = serviceProvider.GetService<ITestService>();
            service.Test("service");
            Console.WriteLine("Hello World!");
        }

        
    }
}
