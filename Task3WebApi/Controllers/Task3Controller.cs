using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebExtraction.Model;

namespace Task3WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Task3Controller : ControllerBase
    {
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<HotelRate>))]
        public ActionResult<List<HotelRate>> Get(int hotelId, DateTime arrivalDate)
        {
            using (StreamReader r = new StreamReader("Content/allHotelRates.json"))
            {
                string json = r.ReadToEnd();
                var hotels = JsonSerializer.Deserialize<List<JsonHotelObject>>(json);

                var searchedHotel = hotels.FirstOrDefault(t => t.Hotel.HotelID == hotelId);
                if (searchedHotel != null)
                {
                    var response = searchedHotel.HotelRates.Where(t => t.TargetDay >= arrivalDate).ToList();
                    return Ok(response);
                }
                return Ok(new List<HotelRate>());
            }
            return BadRequest();
        }
    }
}
