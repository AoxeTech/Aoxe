using Aoxe.RabbitMQ;

namespace ServiceHost;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService", Version = "v1" });
        });

        //注册DDD各层
        services
            .AddDDD()
            //注册DbContext
            .AddDbContext(Configuration)
            //注册序列化器用于包装需要持久化的事件
            .AddSingleton<ITextSerializer, Serializer>()
            //注册RabbitMQ以用于消息总线
            .AddSingleton<IMessageBus, MessageBus>()
            .AddAoxeRabbitMq(
                new AoxeRabbitMqOptions
                {
                    AutomaticRecoveryEnabled = true,
                    HeartBeat = TimeSpan.FromMinutes(1),
                    NetworkRecoveryInterval = new TimeSpan(60),
                    Hosts =  ["192.168.78.150"],
                    UserName = "admin",
                    Password = "123",
                    Serializer = new Serializer()
                }
            )
        // .AddHostedService<DomainEventBackgroundService>()
        ;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService v1"));
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<UowMiddleware>();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
