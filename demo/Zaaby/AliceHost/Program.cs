using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AliceHost;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://localhost:5001");
            })
            .Build()
            .Run();
}