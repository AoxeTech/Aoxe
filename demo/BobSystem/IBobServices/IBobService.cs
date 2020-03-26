using System;
using Interfaces;

namespace IBobServices
{
    public interface IBobService : ITest
    {
        string Hello();
        string SayHelloToAlice();
        string SayHelloToCarol();
        Exception ThrowException();
        string PassAppleToAlice(string appleName);
    }
}