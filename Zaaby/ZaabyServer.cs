using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Zaaby.Core;

namespace Zaaby
{
    public class ZaabyServer : IZaabyServer
    {
        public IZaabyServer AddTransient(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddTransient(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddTransient<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScope(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScope<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScope(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScope<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddScoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            throw new NotImplementedException();
        }

        public IZaabyServer AddSingleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        {
            throw new NotImplementedException();
        }

        public IZaabyServer IsIService(Func<Type, bool> isIService)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}