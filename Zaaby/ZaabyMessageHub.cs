using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Zaaby.Abstractions;

namespace Zaaby
{
    //public class ZaabyMessageHub : IZaabyMessageHub
    //{
    //    private readonly IList<Type> _allTypes;
        
    //    private readonly ConcurrentDictionary<Type, ConcurrentBag<Func<Action<TMessage>>>>
    //        _subscriberResolves = new ConcurrentDictionary<Type, ConcurrentBag<Func<Action<TMessage>>>>();

    //    private readonly string _handleName;

    //    public ZaabyMessageHub(Type messageHandlerInterfaceType, Type messageInterfaceType,
    //        string handleName)
    //    {
    //        _allTypes = GetAllTypes();
    //        _handleName = handleName;
    //        RegisterMessageSubscriber(messageHandlerInterfaceType, messageInterfaceType, handleName);
    //    }

    //    public void Publish<TMessage>(TMessage message)
    //    {
    //        var type = typeof(TMessage);
    //        if (!_subscriberResolves.ContainsKey(type)) return;
    //        foreach (var handleMethodType in _subscriberResolves[type])
    //        {
    //            var handler = (IDomainEventHandler) _serviceProvider.GetService(handleMethodType.DeclaringType);
    //            handleMethodType.Invoke(handler, new object[] {message});
    //        }
    //    }

    //    public void Subscribe<TMessage>(Func<Action<TMessage>> handle)
    //    {

    //    }

    //    public void RegisterMessageSubscriber(Type messageHandlerInterfaceType, Type messageInterfaceType,
    //        string handleName)
    //    {
    //        var messageHandlerTypes = _allTypes
    //            .Where(type => type.IsClass && messageHandlerInterfaceType.IsAssignableFrom(type)).ToList();

    //        var messageTypes = _allTypes
    //            .Where(type => type.IsClass && messageInterfaceType.IsAssignableFrom(type)).ToList();

    //        messageHandlerTypes.ForEach(messageHandlerType =>
    //        {
    //            var handleMethods = messageHandlerType.GetMethods()
    //                .Where(m =>
    //                    m.Name == handleName &&
    //                    m.GetParameters().Count() == 1 &&
    //                    messageTypes.Contains(m.GetParameters()[0].ParameterType)
    //                ).ToList();
                
    //            handleMethods.ForEach(handleMethod =>
    //            {
                    
    //            });
    //        });
    //    }

    //    private void Register(Type messageHandlerInterfaceType, Type messageInterfaceType,
    //        string handleName)
    //    {
    //        if (!_subscriberResolves.ContainsKey(messageInterfaceType))
    //            _subscriberResolves.TryAdd(messageInterfaceType, new ConcurrentBag<MethodInfo>());
    //        var handleMethods = messageHandlerInterfaceType.GetMethods().Where(m =>
    //            m.Name == handleName &&
    //            m.GetParameters().Count() == 1 &&
    //            m.GetParameters()[0].ParameterType == messageInterfaceType);
    //        foreach (var handleMethod in handleMethods)
    //            _subscriberResolves[messageInterfaceType].Add(handleMethod);
    //    }

    //    private List<Type> GetAllTypes()
    //    {
    //        var dir = Directory.GetCurrentDirectory();
    //        var files = new List<string>();

    //        files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
    //        files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));

    //        var typeDic = new Dictionary<string, Type>();

    //        foreach (var file in files)
    //        {
    //            try
    //            {
    //                foreach (var type in Assembly.LoadFrom(file).GetTypes())
    //                    if (!typeDic.ContainsKey(type.FullName ??
    //                                             throw new NullReferenceException(nameof(type.FullName))))
    //                        typeDic.Add(type.FullName, type);
    //            }
    //            catch
    //            {
    //                // ignored
    //            }
    //        }

    //        return typeDic.Select(kv => kv.Value).ToList();
    //    }
    ////}
}