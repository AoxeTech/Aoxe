using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BackendForFrontend
{
    public class Program
    {
        public static void Main(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000");
                })
                .Build()
                .Run();
    }
}