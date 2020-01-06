using System;
using IAppleServices;

namespace AppleServices
{
    public class AppleTest : IAppleTest
    {
        private readonly IAppleService _appleService;

        public AppleTest(IAppleService appleService)
        {
            _appleService = appleService;
        }

        public string TestAppleService() => $"现在是“{DateTimeOffset.Now}”，来自AppleService的信息是:\"{_appleService.GetAppleMsg()}\"";
    }
}