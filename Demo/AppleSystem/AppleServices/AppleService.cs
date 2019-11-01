using System;
using IAppleServices;
using IBananaServices;

namespace AppleServices
{
    public class AppleService : IAppleService
    {
        private readonly IBananaService _bananaService;
//        private readonly IZaabyMessageHub _zaabyMessageHub;

        public AppleService(IBananaService bananaService)
        {
            _bananaService = bananaService;
//            _zaabyMessageHub = zaabyMessageHub;
        }

        public int GetInt() => new Random().Next();

        public string GetAppleMsg() => $"这里是AppleService， from “{GetType().Name}” on \"{DateTime.Now}\"";

        public string SayHelloToBanana() =>
            $"这里是 Apple.I get the message \"{_bananaService.GetBananaMsg()}\" from Banana.";

        public void ThrowEx() => new Exception($"Throw by apple service in {DateTime.Now}");

        public void TestAppleExFromBanana() => _bananaService.TestAppleEx();

        public void TestBananaEx() => _bananaService.ThrowEx();

        public BananaDto TestBananaDto() =>
            _bananaService.ThrowBack(new BananaDto {Id = Guid.NewGuid(), Time = DateTime.Now});

        public void TestPublishMessageA(int quantity)
        {
            for (var i = 0; i < quantity; i++)
            {
//                _zaabyMessageHub.Publish(new AppleMessageA());
            }
        }

        public void TestPublishMessageB(int quantity)
        {
            for (var i = 0; i < quantity; i++)
            {
//                _zaabyMessageHub.Publish(new AppleMessageB());
            }
        }

        public void TestApple(Apple apple)
        {
            var i = apple.Id;
        }
    }
}