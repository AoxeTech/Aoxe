// namespace Aoxe.WebApi
// {
//     public class AoxeMessageHub : IAoxeMessageHub
//     {
//         private readonly IList<Type> _allTypes;
//
//         private readonly ConcurrentDictionary<Type, ConcurrentBag<Func<Action<TMessage>>>>
//             _subscriberResolves = new ConcurrentDictionary<Type, ConcurrentBag<Func<Action<TMessage>>>>();
//
//         private readonly string _handleName;
//
//         public AoxeMessageHub(Type messageHandlerInterfaceType, Type messageInterfaceType,
//             string handleName)
//         {
//             _allTypes = LoadHelper.GetAllTypes();
//             _handleName = handleName;
//             RegisterMessageSubscriber(messageHandlerInterfaceType, messageInterfaceType, handleName);
//         }
//
//         public void Publish<TMessage>(TMessage message)
//         {
//             var type = typeof(TMessage);
//             if (!_subscriberResolves.ContainsKey(type)) return;
//             foreach (var handleMethodType in _subscriberResolves[type])
//             {
//                 var handler = (IDomainEventHandler) _serviceProvider.GetService(handleMethodType.DeclaringType);
//                 handleMethodType.Invoke(handler, new object[] {message});
//             }
//         }
//
//         public void Subscribe<TMessage>(Func<Action<TMessage>> handle)
//         {
//
//         }
//
//         public void RegisterMessageSubscriber(Type messageHandlerInterfaceType, Type messageInterfaceType,
//             string handleName)
//         {
//             var messageHandlerTypes = _allTypes
//                 .Where(type => type.IsClass && messageHandlerInterfaceType.IsAssignableFrom(type)).ToList();
//
//             var messageTypes = _allTypes
//                 .Where(type => type.IsClass && messageInterfaceType.IsAssignableFrom(type)).ToList();
//
//             messageHandlerTypes.ForEach(messageHandlerType =>
//             {
//                 var handleMethods = messageHandlerType.GetMethods()
//                     .Where(m =>
//                         m.Name is handleName &&
//                         m.GetParameters().Count() is 1 &&
//                         messageTypes.Contains(m.GetParameters()[0].ParameterType)
//                     ).ToList();
//
//                 handleMethods.ForEach(handleMethod => { });
//             });
//         }
//
//         private void Register(Type messageHandlerInterfaceType, Type messageInterfaceType, string handleName)
//         {
//             if (!_subscriberResolves.ContainsKey(messageInterfaceType))
//                 _subscriberResolves.TryAdd(messageInterfaceType, new ConcurrentBag<MethodInfo>());
//             var handleMethods = messageHandlerInterfaceType.GetMethods().Where(m =>
//                 m.Name is handleName &&
//                 m.GetParameters().Count() is 1 &&
//                 m.GetParameters()[0].ParameterType is messageInterfaceType);
//             foreach (var handleMethod in handleMethods)
//                 _subscriberResolves[messageInterfaceType].Add(handleMethod);
//         }
//     }
// }
