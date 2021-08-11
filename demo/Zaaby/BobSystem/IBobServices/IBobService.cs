using System;
using System.Threading.Tasks;
using Interfaces;

namespace IBobServices
{
    public interface IBobService : IService
    {
        string Hello();
        Task<string> HelloTestAsync();
        string SayHelloToAlice();
        Task<string> SayHelloToAliceAsyncTest();
        string SayHelloToCarol();
        Exception ThrowException();
        string PassAppleToAlice(string appleName);
    }
}