using Aoxe;
using Aoxe.Server;
using Aoxe.ThreeTier.Abstractions.BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            AoxeHost
                .Instance
                .AddAoxeService<IBll>()
                .AddThreeTiers()
                .ConfigureServices(services =>
                {
                    services.AddSwaggerDocument();
                })
                .Configure(app =>
                {
                    app.UseHttpsRedirection()
                        .UseOpenApi()
                        .UseSwaggerUi()
                        .UseAoxe()
                        .UseRouting()
                        .UseAuthorization()
                        .UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                })
                .Run();
        }
    }
}
