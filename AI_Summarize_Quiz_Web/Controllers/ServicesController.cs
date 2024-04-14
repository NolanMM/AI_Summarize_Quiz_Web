using AI_Summarize_Quiz_Web.Models;
using Syncfusion.Pdf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using System.IO;
namespace AI_Summarize_Quiz_Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public ServicesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadFile_AI_Module(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "UploadedFiles");

                    // Ensure the directory exists, create it if it doesn't
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string _path = Path.Combine(uploadsFolder, _FileName);
                    using (var stream = new FileStream(_path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    ViewBag.FileName = _FileName;
                    return View();
                }
                else
                {
                    ViewBag.Message = "Please upload a file.";
                    return RedirectToAction("UploadFile", "Services");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed. Exception: " + ex.Message;
                return RedirectToAction("UploadFile", "Services");
            }
        }

        public static string CreateOutputFilePath()
        {
            String timeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
            return ("/output/create" + timeStamp + ".pdf");
        }
        
    }
}
