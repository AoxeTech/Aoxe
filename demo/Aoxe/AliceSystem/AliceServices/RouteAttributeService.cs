using System;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AliceServices
{
    [Route("RouteTest/[action]")]
    [ServiceAttribute]
    public class RouteAttributeService
    {
        public string RouteAttributeTest()
        {
            return $"This has not implemented any interface.[{DateTime.UtcNow}]";
        }
    }
}