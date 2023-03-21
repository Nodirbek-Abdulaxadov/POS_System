using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    public IActionResult Ping()
        => Ok();
}