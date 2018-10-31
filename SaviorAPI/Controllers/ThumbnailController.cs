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
    public class ThumbnailController : ComputerVisionAPIController
    {

        /// <summary>
        /// Get a thumbnail image with the user-specified width and height from an Image file
        /// </summary>
        /// <returns>Image binary</returns>
        /// <param name="width">Width of the thumbnail in pixels. It must be between 1 and 1024. Recommended minimum of 50 </param>
        /// <param name="height">Height of the thumbnail in pixels. It must be between 1 and 1024. Recommended minimum of 50</param>
        /// <param name="smartCropping">Boolean flag for enabling smart cropping</param>
        /// <remarks>
        /// This operation generates a thumbnail image with the user-specified width and height. 
        /// By default, the service analyzes the image, identifies the region of interest (ROI), and generates smart cropping 
        /// coordinates based on the ROI. 
        /// Smart cropping helps when you specify an aspect ratio that differs from that of the input image.
        /// A successful response contains the thumbnail image binary.
        /// If the request failed, the response contains an error code and a message to help determine what went wrong.
        /// Upon failure, the error code and an error message are returned.
        /// The error code could be one of InvalidImageUrl, InvalidImageFormat, InvalidImageSize, InvalidThumbnailSize, NotSupportedImage, FailedToProcess, Timeout, or InternalServerError.
        /// </remarks>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Thumbnail",
                   Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("GetThumbnail")]
        public async Task<IHttpActionResult> GetThumbnail(int width, int height, bool smartCropping = true)
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


                byte[] thumbnail = await visionServiceClient.GetThumbnailAsync(fileStream, width, height, smartCropping);
                return Ok(Convert.ToBase64String(thumbnail));


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get a thumbnail image with the user-specified width and height from a valid Image URL
        /// </summary>
        /// <returns>Image binary</returns>
        /// <param name="imageUrl">A valid URL pointing to an Image</param>
        /// <param name="width">Width of the thumbnail in pixels. It must be between 1 and 1024. Recommended minimum of 50 </param>
        /// <param name="height">Height of the thumbnail in pixels. It must be between 1 and 1024. Recommended minimum of 50</param>
        /// <param name="smartCropping">Boolean flag for enabling smart cropping</param>
        /// <remarks>
        /// This operation generates a thumbnail image with the user-specified width and height. 
        /// By default, the service analyzes the image, identifies the region of interest (ROI), and generates smart cropping 
        /// coordinates based on the ROI. 
        /// Smart cropping helps when you specify an aspect ratio that differs from that of the input image.
        /// A successful response contains the thumbnail image binary.
        /// If the request failed, the response contains an error code and a message to help determine what went wrong.
        /// Upon failure, the error code and an error message are returned.
        /// The error code could be one of InvalidImageUrl, InvalidImageFormat, InvalidImageSize, InvalidThumbnailSize, NotSupportedImage, FailedToProcess, Timeout, or InternalServerError.
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Get Thumbnail",
                   Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("GetThumbnailByURL")]
        public async Task<IHttpActionResult> GetThumbnailByURL(string imageUrl, int width, int height, bool smartCropping = true)
        {

            try
            {
                byte[] thumbnail = await visionServiceClient.GetThumbnailAsync(imageUrl, width, height, smartCropping);
                return Ok(Convert.ToBase64String(thumbnail));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
