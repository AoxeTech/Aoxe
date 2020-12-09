using System;
using Interfaces;

namespace IBobServices
{
    public interface IBobService : IService
    {
        string Hello();
        string SayHelloToAlice();
        string SayHelloToCarol();
        Exception ThrowException();
        string PassAppleToAlice(string appleName);
    }
}