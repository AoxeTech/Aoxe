using System;
using IAliceServices;
using IBobServices;
using ICarolServices;

namespace CarolServices
{
    public class CarolService : ICarolService
    {
        private readonly IAliceService _aliceService;
        private readonly IBobService _bobService;

        public CarolService(IAliceService aliceService, IBobService bobService)
        {
            _aliceService = aliceService;
            _bobService = bobService;
        }

        public string Hello() => "Hello,I am Carol.";

        public string SayHelloToAlice() => $"Hi,I am Carol.\r\n{_aliceService.Hello()}";

        public string SayHelloToCarol() => $"Hi,I am Carol.\r\n{_bobService.Hello()}";

        public Exception ThrowException() => throw new Exception("This exception was thrown by Bob.");
    }
}