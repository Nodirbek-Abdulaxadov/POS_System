using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TimeZoneConverter;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("now")]
        public IActionResult Get()
        {
            return Ok(LocalTime());
        }

        private DateTime LocalTime()
        {
            DateTime utcTime = DateTime.UtcNow; // Get current UTC time
            TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo("Pakistan Standard Time"); // Set the time zone
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        }
    }
}
