using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IAliceServices;
using IBobServices;
using ICarolServices;

namespace AliceServices
{
    public class AliceService : IAliceService
    {
        private readonly IBobService _bobService;
        private readonly ICarolService _carolService;

        public AliceService(IBobService bobService, ICarolService carolService)
        {
            _bobService = bobService;
            _carolService = carolService;
        }

        public string Hello() => "Hi,I am Alice.";

        public string SayHelloToBob() => $"Hi,I am Alice.\r\n{_bobService.Hello()}";
        public async Task<string> SayHelloToBobAsyncTest() => $"Hi,I am Alice.\r\n{await _bobService.HelloAsyncTest()}";

        public string SayHelloToCarol() => $"Hi,I am Alice.\r\n{_carolService.Hello()}";

        public Exception ThrowException() => throw new Exception("This exception was thrown by Alice.");

        public Apple PassBackApple(Apple apple)
        {
            if (apple is null)
                return new Apple
                {
                    Id = 2,
                    Name = "Red Apple",
                    Message = "This apple is pass back by Alice."
                };
            apple.Name ??= string.Empty;
            apple.Message ??= string.Empty;
            apple.Message += "\r\nThis apple is pass back by Alice.";
            return apple;
        }

        public Task<Apple> PassBackAppleAsyncTest(Apple apple)
        {
            if (apple is null)
                return Task.FromResult(new Apple
                {
                    Id = 2,
                    Name = "Red Apple",
                    Message = "This apple is pass back by Alice."
                });
            apple.Name ??= string.Empty;
            apple.Message ??= string.Empty;
            apple.Message += $"\r\nThis apple is pass back by Alice on {DateTime.UtcNow}.";
            return Task.FromResult(apple);
        }

        public async Task<string> HelloAsyncTest()
        {
            var hello = $"Hi,I am Alice.{DateTime.Now}";
            var ms = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(hello);
            await ms.WriteAsync(bytes.AsMemory(0, bytes.Length));
            ms.Position = 0;
            var buffer = new byte[bytes.Length];
            var i = await ms.ReadAsync(buffer.AsMemory(0, buffer.Length));
            var result = Encoding.UTF8.GetString(buffer);
            return result;
        }
    }
}