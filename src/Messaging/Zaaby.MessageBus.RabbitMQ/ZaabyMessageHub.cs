using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.Common;

namespace Zaaby.MessageBus.RabbitMQ
{
    public class ZaabyMessageHub : IZaabyMessageHub
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IZaabeeRabbitMqClient _rabbitMqClient;
        private readonly ushort _prefetch;

        private readonly ConcurrentDictionary<Type, string> _queueNameDic =
            new ConcurrentDictionary<Type, string>();

        public ZaabyMessageHub(IServiceScopeFactory serviceScopeFactory, IZaabeeRabbitMqClient rabbitMqClient,
            MessageHubConfig messageHubConfig)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitMqClient = rabbitMqClient;
            _prefetch = messageHubConfig.Prefetch;

            RegisterMessageSubscriber(messageHubConfig.MessageHandlerInterfaceType,
                messageHubConfig.MessageInterfaceType, messageHubConfig.HandleName);
        }

        public void Publish<TMessage>(TMessage message) =>
            _rabbitMqClient.PublishEvent(message);

        public Task PublishAsync<TMessage>(TMessage message) =>
            _rabbitMqClient.PublishEventAsync(message);

        public void Subscribe<TMessage>(Func<Action<TMessage>> resolve) =>
            _rabbitMqClient.SubscribeEvent(resolve);

        public void RegisterMessageSubscriber(Type messageHandlerInterfaceType, Type messageInterfaceType,
            string handleName)
        {
            var messageHandlerTypes = LoadHelper.LoadTypes()
                .Where(type => type.IsClass && messageHandlerInterfaceType.IsAssignableFrom(type)).ToList();

            var messageTypes = LoadHelper.LoadTypes()
                .Where(type => type.IsClass && messageInterfaceType.IsAssignableFrom(type)).ToList();

            var rabbitMqClientType = _rabbitMqClient.GetType();
            var subscribeMethod = rabbitMqClientType.GetMethods().First(m =>
                m.Name == "SubscribeEvent" &&
                m.GetParameters()[0].Name == "exchange" &&
                m.GetParameters()[1].Name == "queue" &&
                m.GetParameters()[2].ParameterType.ContainsGenericParameters &&
                m.GetParameters()[2].ParameterType.GetGenericTypeDefinition() == typeof(Action<>));

            messageHandlerTypes.ForEach(messageHandlerType =>
            {
                var messageHandleMethods = messageHandlerType.GetMethods()
                    .Where(m =>
                        m.Name == handleName &&
                        m.GetParameters().Count() == 1 &&
                        messageTypes.Contains(m.GetParameters()[0].ParameterType)
                    ).ToList();

                messageHandleMethods.ForEach(handleMethod =>
                {
                    var messageType = handleMethod.GetParameters()[0].ParameterType;

                    var paramTypeName = GetTypeName(messageType);
                    var exchangeName = paramTypeName;
                    var queueName = GetQueueName(handleMethod, paramTypeName);

                    void HandleAction(object message)
                    {
                        var actionT = typeof(Action<>).MakeGenericType(messageType);
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var handler = scope.ServiceProvider.GetService(messageHandlerType);
                            var @delegate = Delegate.CreateDelegate(actionT, handler, handleMethod);
                            @delegate.Method.Invoke(handler, new[] {message});
                        }
                    }

                    subscribeMethod.MakeGenericMethod(messageType)
                        .Invoke(_rabbitMqClient,
                            new object[] {exchangeName, queueName, (Action<object>) HandleAction, _prefetch});
                });
            });
        }

        private string GetTypeName(Type type) => _queueNameDic.GetOrAdd(type,
            key => !(type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault() is
                MessageVersionAttribute msgVerAttr)
                ? type.ToString()
                : $"{type}[{msgVerAttr.Version}]");

        private static string GetQueueName(MemberInfo memberInfo, string eventName) =>
            $"{memberInfo.ReflectedType?.FullName}.{memberInfo.Name}[{eventName}]";
    }
}