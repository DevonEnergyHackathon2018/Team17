using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SaviorAPI.Controllers
{
    public abstract class FaceApiController : ApiController
    {
        protected FaceServiceClient faceServiceClient;
        public FaceApiController()
        {
            faceServiceClient = new FaceServiceClient(
                ConfigurationManager.AppSettings["FaceAPIKey"],
                ConfigurationManager.AppSettings["FaceAPIRoot"]);
        }
    }
}