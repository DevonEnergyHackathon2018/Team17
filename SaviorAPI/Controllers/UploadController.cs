using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Swashbuckle.Swagger.Annotations;

namespace SaviorAPI.Controllers
{
    /// <summary>
    /// Upload the image to a blob storage and return the details of the uploaded file
    /// </summary>
    public class UploadController : ApiController
    {
        /// <summary>
        /// Upload the image to the blob storage
        /// </summary>
        /// <remarks>Upload the image to the blob storage and returns the UploadedFileInfo</remarks>
        /// <returns>UploadedFileInfo</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK,
                Description = "Created",
                Type = typeof(UploadedFileInfo))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                Description = "Internal Server Error",
               Type = typeof(Exception))]
        [SwaggerOperation("Upload")]
        public async Task<IHttpActionResult> PostFormData()
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

                var extension = ExtractExtension(file1);
                var contentType = file1.Headers.ContentType.ToString();
                var imageName = string.Concat(Guid.NewGuid().ToString(), extension);
                var storageConnectionString = "";// ";
                storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();

                var blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = contentType;
                blockBlob.UploadFromStream(fileStream);

                var fileInfo = new UploadedFileInfo
                {
                    FileName = imageName,
                    FileExtension = extension,
                    ContentType = contentType,
                    FileURL = blockBlob.Uri.ToString()
                };
                return Ok(fileInfo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private static string ExtractExtension(HttpContent file)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var fileStreamName = file.Headers.ContentDisposition.FileName;
            var fileName = new string(fileStreamName.Where(x => !invalidChars.Contains(x)).ToArray());
            var extension = Path.GetExtension(fileName);

            return extension;
        }
    }
}
