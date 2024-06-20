using System.Collections.Generic;
using Aoxe;
using Aoxe.AspNetCore.Formatters.Jil;
using Aoxe.AspNetCore.Formatters.MsgPack;
using Aoxe.AspNetCore.Formatters.Protobuf;
using Aoxe.AspNetCore.Formatters.Utf8Json;
using Aoxe.AspNetCore.Formatters.ZeroFormatter;
using Aoxe.Client.Http.MsgPack;
using Aoxe.Server;
using BobServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BobHost;

class Program
{
    public static void Main(string[] args)
    {
        AoxeHost
            .Instance
            .FromAssemblyOf<IAliceService>()
            .FromAssemblyOf(typeof(IBobService))
            .FromAssemblies(typeof(ICarolService).Assembly)
            .FromAssemblyNames(typeof(BobService).Assembly.GetName())
            .AddAoxeService<IService>()
            .UseAoxeClient(
                typeof(IService),
                new Dictionary<string, string>
                {
                    { "IAliceServices", "http://localhost:5001" },
                    { "ICarolServices", "http://localhost:5003" }
                },
                options => options.UseMsgPackFormatter()
            )
            .ConfigureServices(services =>
            {
                services
                    .AddControllers()
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
                    .UseAoxe()
                    .UseRouting()
                    .UseAuthorization()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
            })
            .UseUrls("http://localhost:5002")
            .Run();
    }
}
