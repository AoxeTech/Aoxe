using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CarolHost;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://localhost:5003");
            })
            .Build()
            .Run();
}