using System;
using System.Collections.Generic;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Zaabee.NewtonsoftJson;
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
                    {"IAliceServices", new List<string> {"http://localhost:5001"}},
                    {"IBobServices", new List<string> {"http://localhost:5002"}},
                    {"ICarolServices", new List<string> {"http://localhost:5003"}}
                });
            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    if (string.Equals(input, "exit", StringComparison.CurrentCultureIgnoreCase)) break;
                    var aliceService = client.GetService<IAliceService>();
                    var bobService = client.GetService<IBobService>();
                    var carolService = client.GetService<ICarolService>();

                    if (string.Equals(input, "Hello", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(aliceService.Hello());
                    if (string.Equals(input, "SayHelloToBob", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(aliceService.SayHelloToBob());
                    else if (string.Equals(input, "SayHelloToAlice", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(bobService.SayHelloToAlice());
                    else if (string.Equals(input, "PassBack", StringComparison.CurrentCultureIgnoreCase))
                        Console.WriteLine(
                            aliceService.PassBackApple(new Apple {Id = 3, Name = "Green Apple"}).ToJson());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}