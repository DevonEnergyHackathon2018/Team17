using System;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;


namespace SaviorAPI.Controllers
{
    /// <summary>
    /// Measurement Controller
    /// </summary>
    public class MeasurementController : ApiController
    {
        /// <summary>
        /// Measures the current GPS location
        /// </summary>
        /// <remarks>
        /// Measures the current GPS location
        /// </remarks>
        /// <returns>Measured distance in meters</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
                   Description = "Measurement",
                   Type = typeof(Int32))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                   Description = "Internal Server Error",
                   Type = typeof(Exception))]
        [SwaggerOperation("Measure")]
        public async Task<IHttpActionResult> Measure(double long1, double lat1, double long2, double lat2 )
        {

            try
            {


                lat1 *= System.Math.PI / 180;
                lat2 *= System.Math.PI / 180;
                long1 *= System.Math.PI / 180;
                long2 *= System.Math.PI / 180;

                double dlong = (long2 - long1);
                double dlat = (lat2 - lat1);

                // Haversine formula:
                double R = 6371;
                double a = System.Math.Sin(dlat / 2) * System.Math.Sin(dlat / 2) + System.Math.Cos(lat1) * System.Math.Cos(lat2) * System.Math.Sin(dlong / 2) * System.Math.Sin(dlong / 2);
                double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
                double d = R * c;

                return Ok(Convert.ToInt32(d*1000.0));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
