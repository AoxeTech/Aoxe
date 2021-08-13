using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby
{
    public partial class ZaabyHost
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors = new();
        private readonly List<ServiceDescriptor> _tryAddEnumerableDescriptors = new();

        #region AddTransient

        public ZaabyHost AddTransient(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Transient);
            return Instance;
        }

        public ZaabyHost AddTransient<TService, TImplementation>() where TImplementation : class, TService =>
            AddTransient(typeof(TService), typeof(TImplementation));

        public ZaabyHost AddTransient(Type implementationType) =>
            AddTransient(implementationType, implementationType);

        public ZaabyHost AddTransient<TService>(Type implementationType) =>
            AddTransient(typeof(TService), implementationType);

        public ZaabyHost AddTransient<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddTransient(implementationType, implementationType);
        }

        public ZaabyHost AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory is null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Transient);
            return Instance;
        }

        public ZaabyHost AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddTransient(typeof(TService), implementationFactory);

        #endregion

        #region AddScoped

        public ZaabyHost AddScoped(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Scoped);
            return Instance;
        }

        public ZaabyHost AddScoped<TService, TImplementation>() where TImplementation : class, TService =>
            AddScoped(typeof(TService), typeof(TImplementation));

        public ZaabyHost AddScoped(Type serviceType) =>
            AddScoped(serviceType, serviceType);

        public ZaabyHost AddScoped<TService>(Type implementationType) =>
            AddScoped(typeof(TService), implementationType);

        public ZaabyHost AddScoped<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddScoped(implementationType, implementationType);
        }

        public ZaabyHost AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory is null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Scoped);
            return Instance;
        }

        public ZaabyHost AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddScoped(typeof(TService), implementationFactory);

        #endregion

        #region AddSingleton

        public ZaabyHost AddSingleton(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Singleton);
            return Instance;
        }

        public ZaabyHost AddSingleton<TService, TImplementation>() where TImplementation : class, TService =>
            AddSingleton(typeof(TService), typeof(TImplementation));

        public ZaabyHost AddSingleton(Type serviceType) =>
            AddSingleton(serviceType, serviceType);

        public ZaabyHost AddSingleton<TService>(Type implementationType) =>
            AddSingleton(typeof(TService), implementationType);

        public ZaabyHost AddSingleton<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddSingleton(implementationType, implementationType);
        }

        public ZaabyHost AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory is null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Singleton);
            return Instance;
        }

        public ZaabyHost AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddSingleton(typeof(TService), implementationFactory);

        #endregion

        private void Add(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
            _serviceDescriptors.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));

        private void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime) =>
            _serviceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));

        public void TryAddEnumerable(params ServiceDescriptor[] descriptors) =>
            _tryAddEnumerableDescriptors.AddRange(descriptors);
    }
}