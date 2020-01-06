using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BackendForFrontend.Models;
using IAppleServices;
using IBananaServices;

namespace BackendForFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppleService _appleService;
        private readonly IBananaService _bananaService;

        public HomeController(IAppleService appleService, IBananaService bananaService)
        {
            _appleService = appleService;
            _bananaService = bananaService;
        }

        public int Index()
        {
            return _appleService.GetInt();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}