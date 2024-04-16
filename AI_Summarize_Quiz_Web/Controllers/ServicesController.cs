using AI_Summarize_Quiz_Web.Models;
using Syncfusion.Pdf;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using System.IO;
using System.Security.Principal;
namespace AI_Summarize_Quiz_Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public FileDictionary fileDictionary;
        private const string UPLOAD_URL = "http://localhost:25000/upload";
        private const string PROCESS_URL = "http://localhost:25000/process";
        public ServicesController(IWebHostEnvironment environment, FileDictionary fileDictionary)
        {
            _environment = environment;
            this.fileDictionary = fileDictionary;
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> UploadFile_AI_Module(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string SessionId = Guid.NewGuid().ToString();
                    string _FileName = Path.GetFileName(file.FileName);

                    // Add the file to the dictionary
                    FileDictionary.Files_Session.Add(SessionId, _FileName);

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

                    byte[] fileBytes = System.IO.File.ReadAllBytes(_path);

                    using (var client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new ByteArrayContent(fileBytes), "file", _FileName);
                            var response = await client.PostAsync(UPLOAD_URL, content);

                            if (response.IsSuccessStatusCode)
                            {
                                var userId = await response.Content.ReadAsStringAsync();
                                dynamic? responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(userId);
                                string userId_ = responseData.user_id;
                                ViewBag.FileName = _FileName;
                                ViewBag.SessionId = userId_;
                                return View();
                            }
                            else
                            {
                                ViewBag.Message = "File upload failed.";
                                return RedirectToAction("UploadFile", "Services");
                            }
                        }
                    }


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
