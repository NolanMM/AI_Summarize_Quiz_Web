using System.Diagnostics;
using AI_Summarize_Quiz_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AI_Summarize_Quiz_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (this.Request.Cookies.ContainsKey("SessionID"))
            {
                this.Response.Cookies.Delete("SessionID");
                this.Response.Cookies.Delete("FileName");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
