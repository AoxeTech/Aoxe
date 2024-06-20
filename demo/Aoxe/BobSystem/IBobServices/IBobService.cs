using System;
using System.Threading.Tasks;
using Interfaces;

namespace IBobServices
{
    public interface IBobService : IService
    {
        string Hello();
        Task<string> HelloAsyncTest();
        string SayHelloToAlice();
        Task<string> SayHelloToAliceAsyncTest();
        string SayHelloToCarol();
        Exception ThrowException();
        Task<string> PassAppleToAliceAsync(string appleName);
    }
}