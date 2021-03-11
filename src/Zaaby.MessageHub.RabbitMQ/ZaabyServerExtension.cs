// using System;
// using System.Linq;
// using Zaabee.RabbitMQ.Abstractions;
// using Zaaby.Service.Abstractions;
//
// namespace Zaaby.Service.MessageHub.RabbitMQ
// {
//     public static class ZaabyServerExtension
//     {
//         public static IZaabyServer UseRabbitMqMessageHub(this IZaabyServer zaabyServer,
//             Func<IServiceProvider, IZaabeeRabbitMqClient> rmqClientFactory,
//             MessageHubConfig messageHubConfig)
//         {
//             zaabyServer.AddSingleton(rmqClientFactory);
//             zaabyServer.AddSingleton(p => messageHubConfig);
//             zaabyServer.RegisterServiceRunner<IZaabyMessageHub, ZaabyMessageHub>();
//
//             var messageHandlerTypes = LoadHelper.GetAllTypes()
//                 .Where(type => type.IsClass && messageHubConfig.MessageHandlerInterfaceType.IsAssignableFrom(type))
//                 .ToList();
//             messageHandlerTypes.ForEach(messageHandlerType => zaabyServer.AddScoped(messageHandlerType));
//             return zaabyServer;
//         }
//     }
// }