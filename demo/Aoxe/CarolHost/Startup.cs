using System.Linq;
using System.Reflection;
using ProtoBuf.Grpc.Server;

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

        // Assuming you have a List<TypePair> called serviceTypes
        List<TypePair> serviceTypes = GetServiceTypes();

        foreach (var typePair in serviceTypes)
        {
            services.AddScoped(typePair.InterfaceType, typePair.ImplementationType);
        }

        // Add code-first gRPC services
        services.AddCodeFirstGrpc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        List<TypePair> serviceTypes = GetServiceTypes();

        app.UseAoxeErrorHandling();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            // Map gRPC services
            foreach (var typePair in serviceTypes)
            {
                // Get the generic method definition
                var mapGrpcServiceMethod = typeof(GrpcEndpointRouteBuilderExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .First(m => m.IsGenericMethod && m.Name == "MapGrpcService");

                // Make the generic method with the specific type
                var genericMethod = mapGrpcServiceMethod.MakeGenericMethod(typePair.InterfaceType);

                // Invoke the method
                genericMethod.Invoke(null, new object[] { endpoints });
            }
        });
    }
}
