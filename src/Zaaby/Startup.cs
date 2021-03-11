using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zaaby.Service;

namespace Zaaby
{
    internal class Startup
    {
        internal static readonly List<ServiceDescriptor> ServiceDescriptors = new();
        internal static readonly List<Type> ServiceRunnerTypes = new();
        internal static Func<Type, bool> Definition;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(ServiceDescriptors);
            services.AddZaaby(Definition);
            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseZaaby();
            ServiceRunnerTypes.ForEach(type => serviceProvider.GetService(type));
        }
    }
}