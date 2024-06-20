// using System;
// using System.Linq;
// using Aoxe.RabbitMQ.Abstractions;
// using Aoxe.Server.Abstractions;
//
// namespace Aoxe.Server.MessageHub.RabbitMQ
// {
//     public static class AoxeServerExtension
//     {
//         public static IAoxeServer UseRabbitMqMessageHub(this IAoxeServer AoxeServer,
//             Func<IServiceProvider, IAoxeRabbitMqClient> rmqClientFactory,
//             MessageHubConfig messageHubConfig)
//         {
//             AoxeServer.AddSingleton(rmqClientFactory);
//             AoxeServer.AddSingleton(p => messageHubConfig);
//             AoxeServer.RegisterServiceRunner<IAoxeMessageHub, AoxeMessageHub>();
//
//             var messageHandlerTypes = LoadHelper.GetAllTypes()
//                 .Where(type => type.IsClass && messageHubConfig.MessageHandlerInterfaceType.IsAssignableFrom(type))
//                 .ToList();
//             messageHandlerTypes.ForEach(messageHandlerType => AoxeServer.AddScoped(messageHandlerType));
//             return AoxeServer;
//         }
//     }
// }