using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Vision;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;
using System.Collections.Generic;

namespace SaviorAPI.Controllers
{
    /// <summary>
    /// Analyzes the image using Cognitive Services Computer Vision API
    /// </summary>
    public class AnalyzeController : ComputerVisionAPIController
    {
        public enum VisualDetails
        {
            Celebrities,
            Landmarks
        }
        private List<VisualFeature> _visualFeatures;
        private List<VisualDetails> _visualDetails; 

                /// <summary>
        /// Analyzes the image using Cognitive Services Computer Vision API
        /// </summary>
        public AnalyzeController(): base()
        {
            
            _visualFeatures = new List<VisualFeature> {
                VisualFeature.Adult,
                VisualFeature.Categories,
                VisualFeature.Color,
                VisualFeature.Description,
                VisualFeature.Faces,
                VisualFeature.ImageType,
                VisualFeature.Tags
            };
            _visualDetails = new List<VisualDetails>
            {
                VisualDetails.Celebrities,
                VisualDetails.Landmarks
            };
        }

        /// <summary>
        /// Analyze the Image by specifying the Image
        /// </summary>
        /// <returns>AnalysisResult</returns>
        /// <param name="visualFeatures">
        /// <br/><b>Categories </b>- categorizes image content according to a taxonomy defined in documentation.
        /// <br/><b>Tags </b>- tags the image with a detailed list of words related to the image content. 
        /// <br/><b>Description</b> - describes the image content with a complete English sentence. 
        /// <br/><b>Faces</b> - detects if faces are present.If present, generate coordinates, gender and age. 
        /// <br/><b>ImageType</b> - detects if image is clipart or a line drawing. 
        /// <br/><b>Color</b> - determines the accent color, dominant color, and whether an image is black &amp; white. 
        /// <br/><b>Adult</b> - detects if the image is pornographic in nature (depicts nudity or a sex act). Sexually suggestive content is also detected 
        /// </param>
        /// <param name="visualDetails">
        /// <br/><b>Celebrities</b>- identifies celebrities if detected in the image.
        /// <br/><b>Landmarks</b>- identifies landmarks if detected in the image 
        /// </param> 
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
        [SwaggerOperation("Analyze")]
        public async Task<IHttpActionResult> Analyze()
        {
            //visualFeatures = visualFeatures ?? _visualFeatures;
            //visualDetails = visualDetails ?? _visualDetails;
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


                AnalysisResult analysisResult = await visionServiceClient.AnalyzeImageAsync(fileStream, _visualFeatures, _visualDetails?.Select(x=>x.ToString()).ToList());
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
        /// <returns>AnalysisResult</returns>
        /// <param name="imageUrl">A valid URL pointing to an Image</param>
        /// <param name="visualFeatures">
        /// <br/><b>Categories </b>- categorizes image content according to a taxonomy defined in documentation.
        /// <br/><b>Tags </b>- tags the image with a detailed list of words related to the image content. 
        /// <br/><b>Description</b> - describes the image content with a complete English sentence. 
        /// <br/><b>Faces</b> - detects if faces are present.If present, generate coordinates, gender and age. 
        /// <br/><b>ImageType</b> - detects if image is clipart or a line drawing. 
        /// <br/><b>Color</b> - determines the accent color, dominant color, and whether an image is black &amp; white. 
        /// <br/><b>Adult</b> - detects if the image is pornographic in nature (depicts nudity or a sex act). Sexually suggestive content is also detected 
        /// </param>
        /// <param name="visualDetails">
        /// <br/><b>Celebrities</b>- identifies celebrities if detected in the image.
        /// <br/><b>Landmarks</b>- identifies landmarks if detected in the image 
        /// </param> 
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
        [SwaggerOperation("AnalyzeURL")]
        public async Task<IHttpActionResult> AnalyzeURL(string imageUrl)
        {
            //visualFeatures = visualFeatures ?? _visualFeatures;
            //visualDetails = visualDetails ?? _visualDetails;
            try
            {
                AnalysisResult analysisResult = await visionServiceClient.AnalyzeImageAsync(imageUrl, _visualFeatures, _visualDetails?.Select(x => x.ToString()).ToList());
                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
