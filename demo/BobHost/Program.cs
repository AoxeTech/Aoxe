using System.Collections.Generic;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Zaaby;
using Zaaby.Service;

namespace BobHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ZaabyServer.Instance
                .AddZaabyService<IService>()
                .UseZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IAliceServices", new List<string> {"https://localhost:5001"}},
                    {"ICarolServices", new List<string> {"http://localhost:5003"}}
                })
                .ConfigureServices(services =>
                {
                    services.AddSwaggerDocument();
                })
                .Configure(app =>
                {
                    app.UseOpenApi();
                    app.UseSwaggerUi3();
                    app.UseZaaby();
                })
                .UseUrls("http://localhost:5002")
                .Run();
        }
    }
}