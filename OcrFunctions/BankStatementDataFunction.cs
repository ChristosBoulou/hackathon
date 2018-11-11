using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OcrFunctions.Dtos;
using OcrFunctions.Services;

namespace OcrFunctions
{
    public class BankStatementDataFunction
    {
        [FunctionName("BankStatementDataFunction")]
        public static async Task<HttpResponseMessage> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
         ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for bank statement data.");


            // Get request body

            Byte[] byteArray = await req.Content.ReadAsByteArrayAsync();
            if (byteArray == null || byteArray.Length == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(@"Empty request body") };
            }

            var textResult = await ComputerVisionHelper.ExtractLocalTextAsync(byteArray, log);

            if (textResult == null || !textResult.Any())
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(@"No text found") };


            }
            // that needs to change to the bank statement thing 
            var bankstatement = textResult.Select(tr => tr.Text).ToList().ExtractBankStatementInfo();
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(bankstatement), Encoding.UTF8, "application/json") };

        }
    }
}

