using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;

namespace HackathonEq2018Ocr.Services
{
    public static class ComputerVisionHelper
    {
        // subscriptionKey = "0123456789abcdef0123456789ABCDEF"
        private const string subscriptionKey = "52b387531b8e48aabd46526e8416a36d";

        // For printed text, change to TextRecognitionMode.Printed
        private const TextRecognitionMode textRecognitionMode =
            TextRecognitionMode.Printed;

        const int numberOfCharsInOperationId = 36;


        // Recognize text from a local image
        public static async Task<IList<Line>> ExtractLocalTextAsync(byte [] uploadedImage
             )
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

           
            // Specify the Azure region
            computerVision.Endpoint = "https://westeurope.api.cognitive.microsoft.com/";
            try
            {
                using (Stream uploadedImageStream = new MemoryStream(uploadedImage))
                {
                    // Start the async process to recognize the text
                    RecognizeTextInStreamHeaders textHeaders =
                      await computerVision.RecognizeTextInStreamAsync(
                                 uploadedImageStream, textRecognitionMode);
                                 
                   return await GetTextAsync(computerVision, textHeaders.OperationLocation);
                }
            }
            catch(Exception ex)
            {
               // logger.LogError(ex, "Problem getting text from computer vision " + ex.Message);
                return new List<Line> { new Line(new List<int> { 0 }, "Error processing message") };
            }


        }

        // Retrieve the recognized text
        private static async Task<IList<Line>> GetTextAsync(ComputerVisionClient computerVision, string operationLocation)
        {
            

            // Retrieve the URI where the recognized text will be
            // stored from the Operation-Location header
            string operationId = operationLocation.Substring(
                operationLocation.Length - numberOfCharsInOperationId);

            Console.WriteLine("\nCalling GetHandwritingRecognitionOperationResultAsync()");
            TextOperationResult result =
                await computerVision.GetTextOperationResultAsync(operationId);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                Console.WriteLine(
                    "Server status: {0}, waiting {1} seconds...", result.Status, i);
                await Task.Delay(200);

                result = await computerVision.GetTextOperationResultAsync(operationId);
            }

            // Display the results
            var lines = result.RecognitionResult.Lines;
            return lines;
        }
    }
    }

