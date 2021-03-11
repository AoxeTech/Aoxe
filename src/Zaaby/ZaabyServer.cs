using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby
{
    public class ZaabyServer
    {
        internal Action<IServiceCollection> ConfigurationServicesAction;
        
        private static readonly object LockObj = new();
        private static ZaabyServer _zaabyServer;

        public static ZaabyServer GetInstance()
        {
            if (_zaabyServer != null) return _zaabyServer;
            lock (LockObj) _zaabyServer ??= new ZaabyServer();
            return _zaabyServer;
        }

        public ZaabyServer AddZaabyService<TService>() =>
            AddZaabyService(type =>
                type.IsInterface && typeof(TService).IsAssignableFrom(type) && type != typeof(TService));

        public ZaabyServer AddZaabyService(Func<Type, bool> definition)
        {
            Startup.Definition = definition;
            return _zaabyServer;
        }

        public ZaabyServer ConfigureServices(Action<IServiceCollection> configureServices)
        {
            ConfigurationServicesAction = configureServices;
            return _zaabyServer;
        }

        #region IOC

        #region AddTransient

        public ZaabyServer AddTransient(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public ZaabyServer AddTransient<TService, TImplementation>() where TImplementation : class, TService =>
            AddTransient(typeof(TService), typeof(TImplementation));

        public ZaabyServer AddTransient(Type implementationType) =>
            AddTransient(implementationType, implementationType);

        public ZaabyServer AddTransient<TService>(Type implementationType) =>
            AddTransient(typeof(TService), implementationType);

        public ZaabyServer AddTransient<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddTransient(implementationType, implementationType);
        }

        public ZaabyServer AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory is null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public ZaabyServer AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddTransient(typeof(TService), implementationFactory);

        #endregion

        #region AddScoped

        public ZaabyServer AddScoped(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public ZaabyServer AddScoped<TService, TImplementation>() where TImplementation : class, TService =>
            AddScoped(typeof(TService), typeof(TImplementation));

        public ZaabyServer AddScoped(Type serviceType) =>
            AddScoped(serviceType, serviceType);

        public ZaabyServer AddScoped<TService>(Type implementationType) =>
            AddScoped(typeof(TService), implementationType);

        public ZaabyServer AddScoped<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddScoped(implementationType, implementationType);
        }

        public ZaabyServer AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory is null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public ZaabyServer AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddScoped(typeof(TService), implementationFactory);

        #endregion

        #region AddSingleton

        public ZaabyServer AddSingleton(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public ZaabyServer AddSingleton<TService, TImplementation>() where TImplementation : class, TService =>
            AddSingleton(typeof(TService), typeof(TImplementation));

        public ZaabyServer AddSingleton(Type serviceType) =>
            AddSingleton(serviceType, serviceType);

        public ZaabyServer AddSingleton<TService>(Type implementationType) =>
            AddSingleton(typeof(TService), implementationType);

        public ZaabyServer AddSingleton<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddSingleton(implementationType, implementationType);
        }

        public ZaabyServer AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory is null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public ZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddSingleton(typeof(TService), implementationFactory);

        #endregion

        #endregion

        private static void Add(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
            Startup.ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));

        private static void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime) =>
            Startup.ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));
    }
}