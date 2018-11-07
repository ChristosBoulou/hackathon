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


            if(file.Any())
            {
                return View("Result", new UploadResultViewModel {
                    FileSize = file[0].Length,
                    FileName = file[0].FileName,
                    FileText = await GetFileText(file[0])
                });

            }
            return View();
        }

        private async Task<string> GetFileText(IFormFile file)
        {

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var lines = await ComputerVisionHelper.ExtractLocalTextAsync(fileBytes);
                return string.Join(" - ", lines.Select(t => t.Text));

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
