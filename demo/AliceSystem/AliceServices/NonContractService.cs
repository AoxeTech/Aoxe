using System;
using Interfaces;

namespace AliceServices
{
    public class NonContractService : IService
    {
        public string NonInterfaceTest()
        {
            return $"This has not implemented any interface.[{DateTime.UtcNow}]";
        }
    }
}