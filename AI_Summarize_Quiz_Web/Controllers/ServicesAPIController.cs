using AI_Summarize_Quiz_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace AI_Summarize_Quiz_Web.Controllers
{
    [ApiController()]
    public class ServicesAPIController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_URL = "http://localhost:25000/upload";
        private const string PROCESS_URL = "http://localhost:25000/process";

        public ServicesAPIController(IWebHostEnvironment environment)
        {
            _environment = environment;
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

        [HttpGet("/UploadFile_AI_Module/{fileName}")]
        public async Task<ActionResult> UploadFile_AI_Module(string fileName)
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

                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new ByteArrayContent(fileBytes), "file", fileName);
                        var response = await client.PostAsync(UPLOAD_URL, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var userId = await response.Content.ReadAsStringAsync();
                            dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(userId);
                            string userId_ = responseData.user_id;
                            ViewBag.Message = "File uploaded successfully with user ID: " + userId_;
                            //return View();
                            var processResponse = await ProcessFile(userId_);
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

                                // Save the processed data to a html file
                                string htmlFilePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", "processed_data" + userId_ + ".html");
                                System.IO.File.WriteAllText(htmlFilePath, values);

                                //Initialize HTML to PDF converter. 
                                HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

                                //Convert URL to PDF document. 
                                PdfDocument document = htmlConverter.Convert(values, "");

                                // Pdf path
                                string pdfFilePath = Path.Combine(_environment.WebRootPath, "UploadedFiles", "processed_data" + userId_ + ".pdf");
                                //Create the filestream to save the PDF document. 
                                using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                                {
                                    // Save and close the PDF document.
                                    document.Save(fileStream);
                                    // return the file path
                                    document.Close(true);
                                }
                                // Prepare the json response
                                var responseJson = new
                                {
                                    pdfFilePath = pdfFilePath
                                };
                                // Return the file path as json
                                return Json(responseJson);
                            }
                            else
                            {
                                ViewBag.Message += "\nError processing the file: " + await processResponse.Content.ReadAsStringAsync();
                                return RedirectToAction("UploadFile", "Services");
                            }
                        }
                        else
                        {
                            ViewBag.Message = "File upload failed. Server responded with status code: " + response.StatusCode;
                            return RedirectToAction("UploadFile", "Services");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed. Exception: " + ex.Message;
                return RedirectToAction("UploadFile", "Services");
            }
        }
    }
}
