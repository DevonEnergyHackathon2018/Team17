using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Vision;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;

namespace SaviorAPI.Controllers
{
    /// <summary>
    /// This technology allows you to detect and extract handwritten text from notes, letters, essays, whiteboards, forms, etc. 
    /// It works with different surfaces and backgrounds, such as white paper, yellow sticky notes, and whiteboards.
    /// </summary>
    public class HandWrittenTextController : ComputerVisionAPIController
    {
        private static readonly TimeSpan QueryWaitTimeInSecond = TimeSpan.FromSeconds(3);
        private static readonly int MaxRetryTimes = 3;
        

        /// <summary>
        /// Recognize Handwritten Text from an Image file
        /// </summary>
        /// <returns>Handwriting Recognition Result</returns>
        /// <remarks>
        /// Use this interface to get the result of a Recognize Handwritten Text operation. 
        /// For the result of a Recognize Handwritten Text operation to be available, it requires an amount of time 
        /// that depends on the length of the text.So, you may need to wait before using this Get Handwritten Text Operation Result interface. 
        /// The time you need to wait may be up to a number of seconds. 
        /// Note: this technology is currently in preview and is only available for English text.
        /// </remarks>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Handwriting Recognition Result",
                   Type = typeof(HandwritingRecognitionOperationResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("RecognizeHandWrittenText")]
        public async Task<IHttpActionResult> RecognizeHandWrittenText()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest();
            }
            try
            {
                var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());

                var files = provider.Files;
                var file1 = files[0];
                var fileStream = await file1.ReadAsStreamAsync();


                HandwritingRecognitionOperationResult handwritingRecognitionOperationResult = await RecognizeAsync(async () => await visionServiceClient.CreateHandwritingRecognitionOperationAsync(fileStream));
                return Ok(handwritingRecognitionOperationResult);


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Recognize Handwritten Text from a valid Image URL
        /// </summary>
        /// <returns>Handwriting Recognition Result</returns>
        /// <param name="imageUrl">A valid URL pointing to an Image</param>
        /// <remarks>
        /// Use this interface to get the result of a Recognize Handwritten Text operation. 
        /// For the result of a Recognize Handwritten Text operation to be available, it requires an amount of time 
        /// that depends on the length of the text.So, you may need to wait before using this Get Handwritten Text Operation Result interface. 
        /// The time you need to wait may be up to a number of seconds. 
        /// Note: this technology is currently in preview and is only available for English text.
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "OCR Results",
                   Type = typeof(HandwritingRecognitionOperationResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("RecognizeHandWrittenTextByURL")]
        public async Task<IHttpActionResult> RecognizeHandWrittenTextByURL(string imageUrl)
        {

            try
            {
                HandwritingRecognitionOperationResult handwritingRecognitionOperationResult = await RecognizeAsync(async () => await visionServiceClient.CreateHandwritingRecognitionOperationAsync(imageUrl));
                return Ok(handwritingRecognitionOperationResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private async Task<HandwritingRecognitionOperationResult> RecognizeAsync(Func<Task<HandwritingRecognitionOperation>> Func)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
         
            HandwritingRecognitionOperationResult result;
            try
            {
                HandwritingRecognitionOperation operation = await Func();

                result = await visionServiceClient.GetHandwritingRecognitionOperationResultAsync(operation);

                int i = 0;
                while ((result.Status == HandwritingRecognitionOperationStatus.Running || result.Status == HandwritingRecognitionOperationStatus.NotStarted) && i++ < MaxRetryTimes)
                {
                    await Task.Delay(QueryWaitTimeInSecond);

                    result = await visionServiceClient.GetHandwritingRecognitionOperationResultAsync(operation);
                }

            }
            catch (ClientException)
            {
                result = new HandwritingRecognitionOperationResult() { Status = HandwritingRecognitionOperationStatus.Failed };
            }

            return result;

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }
    }
}
