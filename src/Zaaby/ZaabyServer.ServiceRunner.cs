using System;
using System.Collections.Generic;

namespace Zaaby
{
    public partial class ZaabyServer
    {
        internal readonly List<Type> ServiceRunnerTypes = new();
        
        public ZaabyServer RegisterServiceRunner(Type serviceType, Type implementationType)
        {
            AddSingleton(serviceType, implementationType);
            ServiceRunnerTypes.Add(serviceType);
            return Instance;
        }

        public ZaabyServer RegisterServiceRunner<TService, TImplementation>() where TImplementation : class, TService =>
            RegisterServiceRunner(typeof(TService), typeof(TImplementation));

        public ZaabyServer RegisterServiceRunner(Type implementationType) =>
            RegisterServiceRunner(implementationType, implementationType);

        public ZaabyServer RegisterServiceRunner<TService>(Type implementationType) =>
            RegisterServiceRunner(typeof(TService), implementationType);

        public ZaabyServer RegisterServiceRunner<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return RegisterServiceRunner(implementationType, implementationType);
        }

        public ZaabyServer RegisterServiceRunner(Type serviceType, Func<IServiceProvider, object> runnerFactory)
        {
            AddSingleton(serviceType, runnerFactory);
            ServiceRunnerTypes.Add(serviceType);
            return Instance;
        }

        public ZaabyServer RegisterServiceRunner<TService>(Func<IServiceProvider, TService> runnerFactory)
            where TService : class =>
            RegisterServiceRunner(typeof(TService), runnerFactory);
    }
}