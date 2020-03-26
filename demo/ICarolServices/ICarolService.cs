using System;
using Interfaces;

namespace ICarolServices
{
    public interface ICarolService : ITest
    {
        string Hello();
        string SayHelloToAlice();
        string SayHelloToCarol();
        Exception ThrowException();
    }
}