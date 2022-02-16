using System.Collections.Generic;
using BobServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Zaaby;
using Zaaby.AspNetCore.Formatters.Jil;
using Zaaby.AspNetCore.Formatters.MsgPack;
using Zaaby.AspNetCore.Formatters.Protobuf;
using Zaaby.AspNetCore.Formatters.Utf8Json;
using Zaaby.AspNetCore.Formatters.ZeroFormatter;
using Zaaby.Client.Http.Formatter.MsgPack;
using Zaaby.Server;

namespace BobHost;

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
            .UseZaabyClient(typeof(IService), new Dictionary<string, string>
            {
                { "IAliceServices", "http://localhost:5001" },
                { "ICarolServices", "http://localhost:5003" }
            }, options => options.UseMsgPackFormatter())
            .ConfigureServices(services =>
            {
                services.AddControllers()
                    .AddJil()
                    .AddMsgPack()
                    .AddProtobuf()
                    .AddUtf8Json()
                    .AddZeroFormatter()
                    .AddXmlSerializerFormatters();
                services.AddSwaggerDocument();
            })
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