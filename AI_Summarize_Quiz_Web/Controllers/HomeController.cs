using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AI_Summarize_Quiz_Web.Models;

namespace AI_Summarize_Quiz_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(_environment.WebRootPath, "UploadedFiles", _FileName);
                    using (var stream = new FileStream(_path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    ViewBag.Message = "File Uploaded Successfully!!";
                    return View();
                }
                ViewBag.Message = "Please upload a file.";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
