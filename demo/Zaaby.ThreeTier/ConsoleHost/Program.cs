using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Zaaby;
using Zaaby.Service;
using Zaaby.ThreeTier.Abstractions.BusinessLogic;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyHost.Instance
                .AddZaabyService<IBll>()
                .AddThreeTiers()
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
                .Run();
        }
    }
}