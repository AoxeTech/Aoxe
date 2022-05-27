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
        Task<Apple> PassBackAppleAsync(Apple apple);
        Task<string> HelloAsyncTest();
    }
}