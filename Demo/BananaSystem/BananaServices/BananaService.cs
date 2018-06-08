using System;
using IBananaServices;

namespace BananaServices
{
    public class BananaService : IBananaService
    {
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public string GetBananaMsg()
        {
            return $"This is from {GetType().Name} on {DateTimeOffset.Now}";
        }

        public BananaDto ThrowBack(BananaDto dto)
        {
            return dto;
        }
    }
}