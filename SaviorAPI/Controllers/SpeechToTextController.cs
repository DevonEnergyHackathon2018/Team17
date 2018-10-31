using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;


namespace SaviorAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SpeechToTextController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Converts Speech to Text",
                   Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("SpeechConvert")]
        public async Task<IHttpActionResult> SpeechConvert()
        {
            //return Ok("Authorized!");

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

                //BinaryReader binaryReader = new BinaryReader(fileStream);

                //byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);

                var client = new HttpClient();

                // Request headers - replace this example key with your valid subscription key.
                client.DefaultRequestHeaders.Add("Prediction-Key", "bccc59b3dc6c49f3bbafc21ad8687a08");

                // Prediction URL - replace this example URL with your valid prediction URL.
                string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/6730901d-3d53-43c9-a1fe-45b5b22e7436/image?iterationId=6b091189-7f82-4baa-914e-11d2d627d4f6";

                HttpResponseMessage response;

                return Ok("OK");
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
