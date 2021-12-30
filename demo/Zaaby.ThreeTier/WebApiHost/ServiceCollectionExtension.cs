using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace WebApiHost
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbConnection(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));
            return services;
        }
    }
}