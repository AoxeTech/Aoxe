using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.Extensions;

namespace Zaaby
{
    public partial class ZaabyServer
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors = new();
        private readonly List<ServiceDescriptor> _tryAddEnumerableDescriptors = new();

        #region AddTransient

        public ZaabyServer AddTransient(Type serviceType, Type implementationType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType is null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Transient);
            return Instance;
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
            return Instance;
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
            return Instance;
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
            return Instance;
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
            return Instance;
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
            return Instance;
        }

        public ZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            AddSingleton(typeof(TService), implementationFactory);

        #endregion

        private void Add(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
            _serviceDescriptors.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));

        private void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime) =>
            _serviceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));

        public void TryAddEnumerable(ServiceDescriptor descriptor) =>
            _tryAddEnumerableDescriptors.Add(descriptor);

        public void TryAddEnumerable(IEnumerable<ServiceDescriptor> descriptors) =>
            descriptors.ForEach(descriptor => _tryAddEnumerableDescriptors.Add(descriptor));
    }
}