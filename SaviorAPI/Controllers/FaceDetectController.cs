using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SaviorAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FaceDetectController : FaceApiController
    {

        private FaceAttributeType[] faceAttributeTypes; 
    
        /// <summary>
        /// 
        /// </summary>
    public FaceDetectController():base()
        {
            
            faceAttributeTypes = new FaceAttributeType[] {
                FaceAttributeType.Age,
                FaceAttributeType.Emotion,
                FaceAttributeType.FacialHair,
                FaceAttributeType.Gender,
                FaceAttributeType.Glasses,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Smile,
                FaceAttributeType.Hair,
                FaceAttributeType.Accessories
            };
        }
        /// <summary>
        /// Detect Faces in the Image by specifying the Image
        /// </summary>
        /// <remarks>
        /// Face API detects up to 64 human faces with high precision face location in an image. 
        /// Face rectangle (left, top, width and height) indicating the face location in the image is returned along with each detected face.
        /// Optionally, face detection extracts a series of face related attributes such as pose, gender, age, head pose, facial hair and glasses. Refer to Face - Detect [https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236] for more details.
        /// </remarks>
        /// <returns>Array of faces detected</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Array of faces detected",
                   Type = typeof(Face[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("Detect")]
        public async Task<IHttpActionResult> Detect()
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


                Face[] faces = await faceServiceClient.DetectAsync(fileStream, false, false, faceAttributeTypes);
                return Ok(faces);


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Detect Faces in the Image by specifying a valid Image URL 
        /// </summary>
        /// <remarks>
        /// Face API detects up to 64 human faces with high precision face location in an image. 
        /// Face rectangle (left, top, width and height) indicating the face location in the image is returned along with each detected face.
        /// Optionally, face detection extracts a series of face related attributes such as pose, gender, age, head pose, facial hair and glasses. Refer to Face - Detect [https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236] for more details.
        /// </remarks>
        /// <param name="imageUrl">A valid Image URL</param>
        /// <returns>Array of faces detected</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Detected",
                   Type = typeof(Face[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("DetectURL")]
        public async Task<IHttpActionResult> DetectURL(string imageUrl)
        {

            try
            {
                Face[] faces = await faceServiceClient.DetectAsync(imageUrl, false, false, faceAttributeTypes);
                return Ok(faces);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
