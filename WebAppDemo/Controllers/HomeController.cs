using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppDemo.Models;

namespace WebAppDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            this.configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile fromfile)
        {
            try
            {
                ViewBag.Error = null;

                var formdata = new MultipartFormDataContent();

                byte[] fileData;
                using (var reader = new BinaryReader(fromfile.OpenReadStream()))
                {
                    fileData = reader.ReadBytes((int)fromfile.OpenReadStream().Length);
                }

                var fileContent = new ByteArrayContent(fileData);


                formdata.Add(fileContent, "pdffile", fromfile.FileName);

                HttpClient client = new HttpClient();

                //var JWToken = HttpContext.Session.GetString("Token");
                //if (!string.IsNullOrEmpty(JWToken))
                //{
                //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken);
                //}

                client.BaseAddress = new Uri(configuration.GetValue<string>("WebAPIUrl").ToString());

                HttpResponseMessage Response;
                string endpoint = "Employee/DocumentAdd";
                Response = (HttpResponseMessage)await client.PostAsync(endpoint, formdata);
                var vResponseHelper = JsonConvert.DeserializeObject<ResponseHelper>(await Response.Content.ReadAsStringAsync());

                if (vResponseHelper.Status == StatusCodes.Status200OK && vResponseHelper.Data != null)
                {
                    var data = Response.Content.ReadAsStringAsync();

                    byte[] bytes = Convert.FromBase64String(vResponseHelper.Data.ToString());

                    return File(bytes, "application/force-download", Path.GetFileName(fromfile.FileName));

                }
                else
                {
                    ViewBag.Error = "Error: " + vResponseHelper.Message;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
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
