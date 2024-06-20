namespace ServiceHost;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        //注册EF用于C端仓储层
        services.AddDbContext<CustomDbContext>(options =>
            options.UseNpgsql(config.GetSection("PgSqlPrimary").Get<string>()));
        //使用上面已注册的pgsql上下文再次注册DbContext以用于框架内注入提交UOW
        services.AddScoped<ZaabyDddContext>(p => p.GetService<CustomDbContext>());
        return services;
    }
}