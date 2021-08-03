using System.Collections.Generic;
using BobServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Zaaby;
using Zaaby.Server;

namespace BobHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyHost.Instance
                .FromAssemblyOf<IAliceService>()
                .FromAssemblyOf(typeof(IBobService))
                .FromAssemblies(typeof(ICarolService).Assembly)
                .FromAssemblyNames(typeof(BobService).Assembly.GetName())
                .AddZaabyService<IService>()
                .UseZaabyClient(typeof(IService), new Dictionary<string, List<string>>
                {
                    {"IAliceServices", new List<string> {"https://localhost:5001"}},
                    {"ICarolServices", new List<string> {"http://localhost:5003"}}
                })
                .ConfigureServices(services => { services.AddSwaggerDocument(); })
                .Configure(app =>
                {
                    app.UseHttpsRedirection()
                        .UseOpenApi()
                        .UseSwaggerUi3()
                        .UseZaaby()
                        .UseRouting()
                        .UseAuthorization()
                        .UseEndpoints(endpoints => { endpoints.MapControllers(); });
                })
                .UseUrls("http://localhost:5002")
                .Run();
        }
    }
}