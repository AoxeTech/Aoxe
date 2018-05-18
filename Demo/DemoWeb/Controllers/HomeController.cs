using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DemoWeb.Models;
using IFinanceApplication;
using IShippingApplication;

namespace DemoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerFinanceApplication _customerFinanceApplication;
        private readonly IFreightApplication _freightApplication;

        public HomeController(ICustomerFinanceApplication customerFinanceApplication,
            IFreightApplication freightApplication)
        {
            _customerFinanceApplication = customerFinanceApplication;
            _freightApplication = freightApplication;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public string Test()
        {
            return $"{_customerFinanceApplication.FinanceSystemTest()}\r\n" +
                   $"From DemoWeb. {DateTimeOffset.Now.UtcTicks}";
        }
    }
}