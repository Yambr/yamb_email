using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;

namespace Yambr.Email.SDK.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void ReagisterAllModules(this ContainerBuilder containerBuilder)
        {
            var assemply = typeof(ContainerBuilderExtensions).Assembly;
            var path = Path.GetDirectoryName(assemply.Location);
            var directoryInfo = new DirectoryInfo(path);
            var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            foreach (var file in directoryInfo.GetFiles("*.dll"))
            {
                try
                {
                    var nextAssembly = Assembly.LoadFrom(file.FullName);
                    if (!assemblies.Contains(nextAssembly))
                    {
                        assemblies.Add(nextAssembly);
                    }
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }

            containerBuilder.RegisterAssemblyModules(assemblies.ToArray());
        }
    }
}
