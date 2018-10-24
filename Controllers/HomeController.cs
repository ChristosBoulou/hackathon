using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HackathonEq2018Ocr.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using HackathonEq2018Ocr.Services;

namespace HackathonEq2018Ocr.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Index")]
        public async Task<IActionResult> Index(List<IFormFile> file)
        {
            long size = file.Sum(f => f.Length);
            MemoryStream memoryStream = new MemoryStream();
            if(file.Any())
            {
                file[0].CopyTo(memoryStream);
                return View("Result", new UploadResultViewModel {
                    FileSize = memoryStream.Capacity,
                    FileName = file[0].FileName,
                    FileText = await GetFileText(memoryStream)
                });

            }
            return View();
        }

        private async Task<string> GetFileText(Stream fileStream)
        {
            var lines = await ComputerVisionHelper.ExtractLocalTextAsync(fileStream);
            return string.Join(" - ", lines.Select(t => t.Text));



        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
