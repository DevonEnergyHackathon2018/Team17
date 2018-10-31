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
    /// Describes the image using Cognitive Services Computer Vision API
    /// </summary>
    public class DescribeController : ComputerVisionAPIController
    {
        
        /// <summary>
        /// Describe the Image by specifying the Image
        /// </summary>
        /// <returns>AnalysisResult</returns>
        /// <remarks>
        /// This operation generates a description of an image in human readable language with complete sentences. 
        /// The description is based on a collection of content tags, which are also returned by the operation. 
        /// More than one description can be generated for each image. Descriptions are ordered by their confidence score. 
        /// All descriptions are in English.
        /// </remarks>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "AnalysisResult",
                   Type = typeof(AnalysisResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("Describe")]
        public async Task<IHttpActionResult> Describe()
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


                AnalysisResult analysisResult = await visionServiceClient.DescribeAsync(fileStream, 3);
                return Ok(analysisResult);


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Describe the Image by specifying the URL
        /// </summary>
        /// <param name="imageUrl">A valid URL pointing to an Image</param>
        /// <returns>AnalysisResult</returns>
        /// <remarks>
        /// This operation generates a description of an image in human readable language with complete sentences. 
        /// The description is based on a collection of content tags, which are also returned by the operation. 
        /// More than one description can be generated for each image. Descriptions are ordered by their confidence score. 
        /// All descriptions are in English.
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "AnalysisResult",
                   Type = typeof(AnalysisResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("DescribeURL")]
        public async Task<IHttpActionResult> DescribeURL(string imageUrl)
        {

            try
            {
                AnalysisResult analysisResult = await visionServiceClient.DescribeAsync(imageUrl, 3);
                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
