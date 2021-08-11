using System;
using System.Threading.Tasks;
using Interfaces;

namespace IAliceServices
{
    public interface IAliceService : IService
    {
        string Hello();
        string SayHelloToBob();
        Task<string> SayHelloToBobAsyncTest();
        string SayHelloToCarol();
        Exception ThrowException();
        Apple PassBackApple(Apple apple);
        Task<string> HelloTestAsync();
    }
}