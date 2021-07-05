using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaaby.Common;

namespace Zaaby.Service
{
    public static partial class ZaabyIServiceCollectionExtensions
    {
        private static readonly ConcurrentDictionary<Type, List<Type>> MessageHandlers = new();

        public static IServiceCollection AddZaabyHandler<THandler, TMessage>(this IServiceCollection services,
            string handleName = "Handle") =>
            services.AddZaabyHandler(typeof(THandler), typeof(TMessage), handleName);

        public static IServiceCollection AddZaabyHandler(this IServiceCollection services, Type baseHandleType,
            Type messageType, string handleName = "Handle")
        {
            var typePairs = LoadHelper.GetByBaseType(baseHandleType);
            foreach (var classType in typePairs.Where(t => t.ImplementationType is not null)
                .Select(t => t.ImplementationType))
            {
                var methods = classType.GetMethods(BindingFlags.Public)
                    .Where(m =>
                    {
                        var methodParams = m.GetParameters();
                        return methodParams.Length is 1
                               && m.Name == handleName
                               && messageType.IsAssignableFrom(methodParams[0].ParameterType);
                    });
                if (!methods.Any()) continue;
                var handlerInterfaceTypes = classType.GetInterfaces().Where(baseHandleType.IsAssignableFrom);
                foreach (var handlerInterfaceType in handlerInterfaceTypes)
                {
                    var messageHandlerTypes = MessageHandlers.GetOrAdd(messageType, _ => new List<Type>());
                    messageHandlerTypes.Add(handlerInterfaceType);
                    services.AddScoped(handlerInterfaceType, classType);
                }
            }

            services.AddHostedService<HostClass>();
            return services;
        }

        public class HostClass : BackgroundService
        {
            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}