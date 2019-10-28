using System;
using IAppleServices;
using IBananaServices;
using Interfaces;

namespace BananaServices
{
    public class BananaService : IBananaService
    {
        private readonly IAppleService _appleService;

        public BananaService(IAppleService appleService)
        {
            _appleService = appleService;
        }

        public Guid GetGuid() => Guid.NewGuid();

        public string GetBananaMsg() => $"这里是 {GetType().Name} on {DateTime.Now}";

        public BananaDto ThrowBack(BananaDto dto) => dto;

        public void ThrowEx() => throw new Exception($"Throw by banana service in {DateTime.Now}");

        public void ThrowFormatterEx() => Convert.ToInt32("");

        public void TestAppleEx() => _appleService.ThrowEx();
    }
}