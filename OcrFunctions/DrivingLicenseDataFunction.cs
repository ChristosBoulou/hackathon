using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OcrFunctions.Dtos;
using OcrFunctions.Services;

namespace OcrFunctions
{
    public class DrivingLicenseDataFunction
    {
        [FunctionName("DrivingLicenseDataFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for driving license data.");


            // Get request body

            Byte[] byteArray = await req.Content.ReadAsByteArrayAsync();
            if (byteArray == null || byteArray.Length == 0)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Empty request body");
            }

            var textResult = await ComputerVisionHelper.ExtractLocalTextAsync(byteArray);
            return req.CreateResponse(HttpStatusCode.OK, new DrivingLicenseDto
            {
                Name = textResult.Any() ? textResult[0].Text : "Nothing came back"
            });
        }
    }
}
