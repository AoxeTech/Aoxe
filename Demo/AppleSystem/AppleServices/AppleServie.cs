using System;
using IAppleServices;
using IBananaServices;

namespace AppleServices
{
    public class AppleServie : IAppleService
    {
        private readonly IBananaService _bananaService;

        public AppleServie(IBananaService bananaService)
        {
            _bananaService = bananaService;
        }

        public int GetInt()
        {
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
    }
}