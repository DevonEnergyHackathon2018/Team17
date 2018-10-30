using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace SaviorAPI.Controllers
{
    public class FaceDetectionController : ApiController
    {
        // GET: FaceDetection
        public string Get(byte[] img)
        {

            return "Authorized";
        }
    }
}