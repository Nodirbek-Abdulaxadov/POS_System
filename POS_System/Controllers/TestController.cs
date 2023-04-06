using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public IActionResult Get()
        {
            var time = DateTime.Now;
            var utcTime = DateTime.UtcNow;
            var universalTime = time.ToUniversalTime();
            var local = utcTime.ToLocalTime();

            var data = new
            {
                time, 
                utcTime,
                universalTime,
                local
            };

            return Ok(data);
        }
    }
}
