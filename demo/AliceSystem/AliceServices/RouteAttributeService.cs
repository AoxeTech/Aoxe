using System;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AliceServices
{
    [Route("RouteTest/[action]")]
    public class RouteAttributeService : IService
    {
        public string RouteAttributeTest()
        {
            return $"This has not implemented any interface.[{DateTime.UtcNow}]";
        }
    }
}