using System;
using Interfaces;

namespace IAliceServices
{
    public interface IAliceService : ITest
    {
        string Hello();
        string SayHelloToBob();
        string SayHelloToCarol();
        Exception ThrowException();
        Apple PassBackApple(Apple apple);
    }
}