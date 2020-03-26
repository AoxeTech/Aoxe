using System;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Zaabee.Utf8Json;

namespace BobService
{
    public class BobService : IBobService
    {
        private readonly IAliceService _aliceService;
        private readonly ICarolService _carolService;

        public BobService(IAliceService aliceService, ICarolService carolService)
        {
            _aliceService = aliceService;
            _carolService = carolService;
        }

        public string Hello() => "Hello,I am Bob.";

        public string SayHelloToAlice() => $"Hi,I am Bob.\r\n{_aliceService.Hello()}";

        public string SayHelloToCarol() => $"Hi,I am Bob.\r\n{_carolService.Hello()}";

        public Exception ThrowException() => throw new Exception("This exception was thrown by Bob.");

        public string PassAppleToAlice(string appleName)
        {
            var apple = new Apple
            {
                Id = 1,
                Name = appleName,
                Message = $"Bob has passed the apple to Alice on {DateTimeOffset.Now}."
            };
            return _aliceService.PassBackApple(apple).ToJson();
        }
    }
}