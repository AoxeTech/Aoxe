using System;
using Interfaces;

namespace ICarolServices
{
    public interface ICarolService : IService
    {
        string Hello();
        string SayHelloToAlice();
        string SayHelloToCarol();
        Exception ThrowException();
    }
}