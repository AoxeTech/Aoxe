using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using QueryService;

namespace BackendForBrowser
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQueryService(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<UserQueryService>()
                .AddClasses(classes =>
                    classes.Where(@class => @class.Name.EndsWith("QueryService", StringComparison.OrdinalIgnoreCase)))
                .AsSelf().WithScopedLifetime());
            return services;
        }

        public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IDbConnection>(
                _ => new NpgsqlConnection(config.GetSection("PgSqlStandby").Get<string>()));
            return services;
        }
    }
}