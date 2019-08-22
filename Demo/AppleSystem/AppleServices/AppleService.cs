using System;
using IAppleServices;
using IBananaServices;
using Zaaby.Abstractions;

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

        public int GetInt()
        {
            _bananaService.ThrowFormatterEx();
            return new Random().Next();
        }

        public string GetAppleMsg()
        {
            return $"This is from {GetType().Name} on {DateTime.Now}";
        }

        public string SayHelloToBanana()
        {
            return $"Hello,here is Apple.I get the message \"{_bananaService.GetBananaMsg()}\" from Banana.";
        }

        public void ThrowEx()
        {
            throw new Exception($"Throw by apple service in {DateTime.Now}");
        }

        public void TestAppleExFromBanana()
        {
            _bananaService.TestAppleEx();
        }

        public void TestBananaEx()
        {
            _bananaService.ThrowEx();
        }

        public BananaDto TestBananaDto()
        {
            return _bananaService.ThrowBack(new BananaDto {Id = Guid.NewGuid(), Time = DateTime.Now});
        }

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
    }
}