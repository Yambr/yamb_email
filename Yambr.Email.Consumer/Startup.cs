using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yambr.SDK.Autofac;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.Email.Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, IHostingEnvironment env)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            var serviceProvider = ConfigureServices(env, services, Configuration);
            var initHandlers = serviceProvider.GetServices<IInitHandler>();
            foreach (var initHandler in initHandlers)
            {
                initHandler.InitComplete();
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();



        }

        private AutofacServiceProvider ConfigureServices(
            IHostingEnvironment env,
            IServiceCollection services,
            IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            services
                .AddLogging(opt =>
                {
                    opt.AddConsole();
                    opt.AddConfiguration(Configuration.GetSection("Logging"));
                });
            services.AddStackExchangeRedisCache(options =>
            {
                var section = Configuration.GetSection(nameof(RedisCache));
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
            containerBuilder.Populate(services);
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
