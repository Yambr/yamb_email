using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.ComponentModel.Enums;
using Module = Autofac.Module;

namespace Yambr.SDK.Autofac
{
    public abstract class AbstractModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = this.GetType();
            Console.WriteLine(assembly.FullName);
            foreach (var type in assembly.Assembly.GetTypes())
            {
                if (!type.IsClass) continue;
                if (type.IsAbstract) continue;
                var serviceAtributes = type.GetCustomAttributes(typeof(ServiceAttribute)).ToArray();
                if (serviceAtributes.Any())
                {
                    RegisterAsService(builder, type, serviceAtributes);
                }
                var componentAtributes = type.GetCustomAttributes(typeof(ComponentAttribute)).ToArray();
                if (componentAtributes.Any())
                {
                    RegisterAsComponent(builder, type, componentAtributes);
                }
            }
           
        }

        private static void RegisterAsService(ContainerBuilder builder, Type type, Attribute[] serviceAtributes)
        {
            var registrationBuilder = builder.RegisterType(type).AsImplementedInterfaces();
            Console.WriteLine(" registered  service " + type.FullName);
            RegisterScope(registrationBuilder, serviceAtributes.Cast<ServiceAttribute>().FirstOrDefault()?.Scope);
        }

        private static void RegisterAsComponent(ContainerBuilder builder, Type type, IEnumerable<Attribute> componentAtributes)
        {
           
            var interfaces = type.GetInterfaces();

            Console.WriteLine(" registered  component " + type.FullName);
            if (interfaces.Any())
            {
                foreach (var baseInterface in interfaces)
                {
                    var registrationBuilder = builder.RegisterType(type);
                    var extensionPointAttributes =
                        baseInterface.GetCustomAttributes(typeof(ExtensionPointAttribute)).Cast<ExtensionPointAttribute>();

                    var pointAttributes = extensionPointAttributes as ExtensionPointAttribute[] ?? extensionPointAttributes.ToArray();
                    if (pointAttributes.Any())
                    {
                        registrationBuilder.As(baseInterface);
                        Console.WriteLine(
                            "  as extensionPoint " + string.Join(", ", interfaces.Select(c => c.FullName)));
                    }

                    RegisterScope(registrationBuilder, pointAttributes.FirstOrDefault()?.Scope);
                }
            }
            
        }

        private static void RegisterScope(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder, Scope? scope)
        {
            switch (scope)
            {
                case Scope.InstancePerDependency:
                    registrationBuilder.InstancePerDependency();
                    break;
                case Scope.SingleInstance:
                    registrationBuilder.SingleInstance();
                    break;
                case Scope.InstancePerLifetimeScope:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                default:
                    registrationBuilder.SingleInstance();
                    break;
            }
            
        }
    }
}
