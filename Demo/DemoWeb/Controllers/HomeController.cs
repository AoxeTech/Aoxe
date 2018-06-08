using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DemoWeb.Models;
using Interfaces;

namespace DemoWeb.Controllers
{
    public class HomeController : Controller
    {
        private ITest _test;
        
        public HomeController(ITest test)
        {
            _test = test;
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
    }
}