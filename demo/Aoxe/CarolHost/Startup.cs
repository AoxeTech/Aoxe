namespace CarolHost;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJil()
            .AddMsgPack()
            .AddProtobuf()
            .AddUtf8Json()
            .AddZeroFormatter()
            .AddXmlSerializerFormatters();
        services.FromAssemblies(typeof(IAliceService).Assembly, typeof(IBobService).Assembly);
        services.FromAssemblyNames(
            typeof(ICarolService).Assembly.GetName(),
            typeof(CarolService).Assembly.GetName()
        );
        services.AddAoxeService<IService>();
        services.AddAoxeClient(
            typeof(IService),
            new Dictionary<string, string>
            {
                { "IAliceServices", "http://localhost:5001" },
                { "IBobServices", "http://localhost:5002" }
            },
            options => options.UseUtf8JsonFormatter()
        );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAoxeErrorHandling();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
