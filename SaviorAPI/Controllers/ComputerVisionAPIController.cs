using Microsoft.ProjectOxford.Vision;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SaviorAPI.Controllers
{
    public abstract class ComputerVisionAPIController : ApiController
    {
        protected VisionServiceClient visionServiceClient;

        public ComputerVisionAPIController()
        {
            //Adding the APIRoot as per latest documentation https://westus.dev.cognitive.microsoft.com/docs/services/56f91f2d778daf23d8ec6739/operations/56f91f2e778daf14a499e1fa
            //Moving the API Root to the Web Config file
            visionServiceClient = new VisionServiceClient(
                ConfigurationManager.AppSettings["VisionAPIKey"],
                ConfigurationManager.AppSettings["VisionAPIRoot"]);
        }
    }
}