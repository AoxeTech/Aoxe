using System;
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
            if (apple.Name is null) apple.Name = string.Empty;
            if (apple.Message is null) apple.Message = string.Empty;
            apple.Message += "\r\nThis apple is pass back by Alice.";
            return apple;
        }
    }
}