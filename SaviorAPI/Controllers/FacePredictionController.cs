using System;
using System.IO;
using System.Threading.Tasks;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace SaviorAPI.Controllers
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class FacePredictionController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Prediction data is returned",
                   Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("Predict")]
        public async Task<IHttpActionResult> Predict()
        {
            return Ok("Authorized!");

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

                BinaryReader binaryReader = new BinaryReader(fileStream);

                byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);

                var client = new HttpClient();

                // Request headers - replace this example key with your valid subscription key.
                client.DefaultRequestHeaders.Add("Prediction-Key", "bccc59b3dc6c49f3bbafc21ad8687a08");

                // Prediction URL - replace this example URL with your valid prediction URL.
                string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/6730901d-3d53-43c9-a1fe-45b5b22e7436/image?iterationId=6b091189-7f82-4baa-914e-11d2d627d4f6";

                HttpResponseMessage response;

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(url, content);

                    //parse the response
                    string respData = await response.Content.ReadAsStringAsync();
                    dynamic returnData = JsonConvert.DeserializeObject(respData);
                    string prob = string.Empty;
                    string name = string.Empty;

                    foreach(var data in returnData.predictions)
                    {
                        prob = data.probability;
                        name = data.tagName;
                        break;
                    }

                    if (Convert.ToInt32(prob) > 0.5)
                    {
                        return Ok(name);
                    }
                    else
                    {
                        return Ok("Unknown");
                    }
                    //return Ok(response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}