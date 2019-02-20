using IAppleServices;
using Interfaces;

namespace AppleServices
{
    public class AppleConsumer : IConsumer<AppleMessageA>, IConsumer<AppleMessageB>
    {
        public void Consume(AppleMessageA message)
        {
            var i = GetHashCode();
        }

        public void Consume(AppleMessageB message)
        {
            var i = GetHashCode();
        }
    }
}