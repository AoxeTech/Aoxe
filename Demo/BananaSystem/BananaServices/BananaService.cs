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
        
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public string GetBananaMsg()
        {
            return $"This is from {GetType().Name} on {DateTime.Now}";
        }

        public BananaDto ThrowBack(BananaDto dto)
        {
            return dto;
        }

        public void ThrowEx()
        {
            throw new Exception($"Throw by banana service in {DateTime.Now}");
        }

        public void TestAppleEx()
        {
            _appleService.ThrowEx();
        }
    }
}