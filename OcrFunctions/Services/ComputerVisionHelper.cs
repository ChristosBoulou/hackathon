using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OcrFunctions.Services
{
    public static class ComputerVisionHelper
    {
        // subscriptionKey = "0123456789abcdef0123456789ABCDEF"
        private const string subscriptionKey = "52b387531b8e48aabd46526e8416a36d";

        // For printed text, change to TextRecognitionMode.Printed
        private const TextRecognitionMode textRecognitionMode =
            TextRecognitionMode.Printed;

        const int numberOfCharsInOperationId = 36;

        // Recognize text from a remote image
        private static async Task ExtractRemoteTextAsync(this
            ComputerVisionClient computerVision, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return;
            }

            // Start the async process to recognize the text
            RecognizeTextHeaders textHeaders =
                await computerVision.RecognizeTextAsync(
                    imageUrl, textRecognitionMode);

         //   await GetTextAsync(computerVision, textHeaders.OperationLocation);
        }

        // Recognize text from a local image
        public static async Task<IList<Line>> ExtractLocalTextAsync(byte [] uploadedImage, ILogger logger
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
                                 
                    return await GetTextAsync(computerVision, textHeaders.OperationLocation, logger);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Problem getting text from computer vision " + ex.Message);


            }
            return null;

        }

        // Retrieve the recognized text
        private static async Task<IList<Line>> GetTextAsync(ComputerVisionClient computerVision, string operationLocation, ILogger logger)
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
                logger.LogInformation(
                    $"Server status: {result.Status}, waiting {i * 500} milliseconds...");
                await Task.Delay(500);

                result = await computerVision.GetTextOperationResultAsync(operationId);
            }

            // Display the results
            if (result.RecognitionResult != null && result.RecognitionResult.Lines != null)
            {
                var lines = result.RecognitionResult.Lines;
                return lines;
            }
            else
            {
                logger.LogError($"Unable to get text operation result: {JsonConvert.SerializeObject(result)}");
                return null;
            }
        }
    }
    }

