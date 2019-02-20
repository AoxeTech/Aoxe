using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zaaby.Abstractions;

namespace Zaaby.MessageHub
{
//    public class ZaabyMessageHub: IZaabyMessageHub
//    {
//        private readonly ConcurrentDictionary<Type, List<Func<Action<TMessage>>>>
//            _subscriberResolves = new ConcurrentDictionary<Type, List<Func<Action<TMessage>>>>();
//
//        public void Publish<TMessage>(TMessage message)
//        {
//            var type = typeof(TMessage);
//            if (!_subscriberResolves.ContainsKey(type)) return;
//            _subscriberResolves[type].ForEach(handleMethodType =>
//            {
//                handleMethodType.Invoke()(message);
//            });
//        }
//        
//        public void Subscribe<TMessage>(Func<Action<TMessage>> handle)
//        {
//            var type = typeof(TMessage);
//            var functions = _subscriberResolves.GetOrAdd(type, key => new List<Func<Action<TMessage>>>());
//            functions.Add(handle);
//        }
//    }
}