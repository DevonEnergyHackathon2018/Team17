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
    /// Tags the image using Cognitive Services Computer Vision API
    /// </summary>
    public class ImageTagsController : ComputerVisionAPIController
    {
        
        /// <summary>
        /// Get Tags for the Image by specifying the Image file
        /// </summary>
        /// <returns>AnalysisResult</returns>
        /// <remarks>
        /// This operation generates a list of words, or tags, that are relevant to the content of the supplied image. 
        /// The Computer Vision API can return tags based on objects, living beings, scenery or actions found in images. 
        /// Unlike categories, tags are not organized according to a hierarchical classification system, but correspond to image content. 
        /// Tags may contain hints to avoid ambiguity or provide context, for example the tag “cello” may be accompanied by 
        /// the hint “musical instrument”. All tags are in English.
        /// </remarks>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "AnalysisResult",
                   Type = typeof(AnalysisResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("GetTags")]
        public async Task<IHttpActionResult> GetTags()
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


                AnalysisResult analysisResult = await visionServiceClient.GetTagsAsync(fileStream);
                return Ok(analysisResult);


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get Tags for the Image by specifying the URL
        /// </summary>
        /// <param name="imageUrl">A valid URL pointing to an Image</param>
        /// <returns>AnalysisResult</returns>
        /// <remarks>
        /// This operation generates a list of words, or tags, that are relevant to the content of the supplied image. 
        /// The Computer Vision API can return tags based on objects, living beings, scenery or actions found in images. 
        /// Unlike categories, tags are not organized according to a hierarchical classification system, but correspond to image content. 
        /// Tags may contain hints to avoid ambiguity or provide context, for example the tag “cello” may be accompanied by 
        /// the hint “musical instrument”. All tags are in English.
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "AnalysisResult",
                   Type = typeof(AnalysisResult))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("GetTagsByURL")]
        public async Task<IHttpActionResult> GetTagsByURL(string imageUrl)
        {

            try
            {
                AnalysisResult analysisResult = await visionServiceClient.GetTagsAsync(imageUrl);
                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
