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
    /// OCR technology detects text content in an image and extracts the identified text into a machine-readable character stream
    /// </summary>
    public class OCRController : ComputerVisionAPIController
    {

        /// <summary>
        /// Recognize Text in the Image by specifying the Image
        /// </summary>
        /// <returns>OCR Results</returns>
        /// <remarks>
        /// Optical Character Recognition (OCR) detects text in an image and extracts the recognized characters into a 
        /// machine-usable character stream. 
        /// Upon success, the OCR results will be returned.
        /// Upon failure, the error code together with an error message will be returned.
        /// The error code can be one of InvalidImageUrl, InvalidImageFormat, InvalidImageSize, NotSupportedImage, NotSupportedLanguage, or InternalServerError.
        /// </remarks>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "OCR Results",
                   Type = typeof(OcrResults))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("RecognizeText")]
        public async Task<IHttpActionResult> RecognizeText()
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


                OcrResults ocrResults = await visionServiceClient.RecognizeTextAsync(fileStream, "unk", true); 
                return Ok(ocrResults);


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Recognize Text in the Image by specifying the Image URL
        /// </summary>
        /// <param name="imageUrl">A valid URL pointing to an Image</param>
        /// <returns>AnalysisResult</returns>
        /// <remarks>
        /// Optical Character Recognition (OCR) detects text in an image and extracts the recognized characters into a 
        /// machine-usable character stream. 
        /// Upon success, the OCR results will be returned.
        /// Upon failure, the error code together with an error message will be returned.
        /// The error code can be one of InvalidImageUrl, InvalidImageFormat, InvalidImageSize, NotSupportedImage, NotSupportedLanguage, or InternalServerError.
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "OCR Results",
                   Type = typeof(OcrResults))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("RecognizeTextByURL")]
        public async Task<IHttpActionResult> RecognizeTextByURL(string imageUrl)
        {

            try
            {
                OcrResults ocrResults = await visionServiceClient.RecognizeTextAsync(imageUrl, "unk", true);
                return Ok(ocrResults);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
