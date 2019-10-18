using System;
using System.Collections.Generic;
using IAppleServices;
using IBananaServices;
using Zaabee.SystemTextJson;
using Zaaby.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client running......");
            var client =
                new ZaabyClient(new Dictionary<string, List<string>>
                {
                    {"IAppleServices", new List<string> {"http://localhost:5000"}},
                    {"IBananaServices", new List<string> {"http://localhost:5001"}}
                });
            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    if (string.Equals(input, "exit", StringComparison.CurrentCultureIgnoreCase)) break;
                    var appleService = client.GetService<IAppleService>();
                    var bananaService = client.GetService<IBananaService>();

                    if (string.Equals(input, "GetInt", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(appleService.GetInt());
                    if (string.Equals(input, "GetGuid", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(bananaService.GetGuid());
                    else if (string.Equals(input, "SayHelloToBanana", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(appleService.SayHelloToBanana());
                    else if (string.Equals(input, "ThrowBack", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(
                            bananaService.ThrowBack(new BananaDto {Id = Guid.NewGuid(), Time = DateTime.Now})
                                .ToJson());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}