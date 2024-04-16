using AI_Summarize_Quiz_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Text.Json;

namespace AI_Summarize_Quiz_Web.Controllers
{
    [ApiController()]
    public class ServicesAPIController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_URL = "http://localhost:25000/upload";
        private const string PROCESS_URL = "http://localhost:25000/process";
        private FileDictionary fileDictionary;
        public ServicesAPIController(IWebHostEnvironment environment, FileDictionary fileDictionary)
        {
            _environment = environment;
            this.fileDictionary = fileDictionary;
        }

        private async Task<HttpResponseMessage> ProcessFile(string userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var cookie = new System.Net.Cookie("user_id", userId);
                    var cookieContainer = new System.Net.CookieContainer();
                    cookieContainer.Add(new Uri(PROCESS_URL), cookie);
                    client.DefaultRequestHeaders.Add("Cookie", $"user_id={userId}");
                    return await client.PostAsync(PROCESS_URL, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("/UploadFile_AI_Module_API/{fileName}/{SessionId}")]
        public async Task<ActionResult> UploadFile_AI_Module(string fileName, string SessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    ViewBag.Message = "File is empty or invalid.";
                    return RedirectToAction("UploadFile", "Services");
                }

                string filePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", fileName);
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                
                var processResponse = await ProcessFile(SessionId);
                if (processResponse.IsSuccessStatusCode)
                {
                    ViewBag.Message += "\nFile processed successfully.";
                    string data_response_ai_raw = await processResponse.Content.ReadAsStringAsync();
                    ViewBag.ProcessedData = data_response_ai_raw;
                    var response_ = JsonConvert.DeserializeObject<ResponseAIDataModel>(data_response_ai_raw);
                    //var mockExam = response_.Results.MockExam;
                    var mockExam = response_.Results;
                    dynamic mock_exam_key = Newtonsoft.Json.JsonConvert.DeserializeObject(mockExam);
                    string values_ = mock_exam_key.mock_exam;
                    string values = "<pre>" + values_ + "</pre>";

                    // Save the processed data to a text file
                    string TextFilePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", "MockExam_" + SessionId + ".txt");
                    System.IO.File.WriteAllText(TextFilePath, values_);

                    // Save the processed data to a html file
                    string htmlFilePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", "MockExam_" + SessionId + ".html");
                    System.IO.File.WriteAllText(htmlFilePath, values);

                    //Initialize HTML to PDF converter. 
                    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

                    //Convert URL to PDF document. 
                    PdfDocument document = htmlConverter.Convert(values, "");

                    // Pdf path
                    string pdfFilePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", "MockExam_" + SessionId + ".pdf");
                    //Create the filestream to save the PDF document. 
                    using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        // Save and close the PDF document.
                        document.Save(fileStream);
                        // return the file path
                        document.Close(true);
                    }
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                    };
                    // Return Json success with SessionId

                    var SessionId_packages = new { SessionId = SessionId };
                    return Json(SessionId_packages, options);
                }
                else
                {
                    ViewBag.Message += "\nError processing the file: " + await processResponse.Content.ReadAsStringAsync();
                    return RedirectToAction("UploadFile", "Services");
                } 
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed. Exception: " + ex.Message;
                return RedirectToAction("UploadFile", "Services");
            }
        }

        [HttpGet("/Download/{file}")]
        public IActionResult Download(string file)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", file);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", file);
        }

        // Get text file return json
        [HttpGet("/DownloadText/{file}")]
        public IActionResult DownloadText(string file)
        {
            // Check if file contain txt
            if (!file.Contains(".txt"))
            {
                return Json("File is not a text file");
            }
            string filePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", file);
            string fileText = System.IO.File.ReadAllText(filePath);
            return Json(fileText);
        }

        [HttpGet("/CheckFile/{file}")]
        public IActionResult CheckFile(string file)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", file);
            if (System.IO.File.Exists(filePath))
            {
                return Json("File exist");
            }
            else
            {
                return Json("File does not exist");
            }
        }
    }
}
