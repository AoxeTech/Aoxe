using IBananaServices;
using Interfaces;

namespace IAppleServices
{
    public interface IAppleService : ITest
    {
        int GetInt();
        string GetAppleMsg();
        string SayHelloToBanana();
        void ThrowEx();
        void TestAppleExFromBanana();
        void TestBananaEx();
        BananaDto TestBananaDto();
    }
}