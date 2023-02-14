using BLL.Dtos.Identity;
using BLL.Interfaces;
using BLL.Validations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserViewModel viewModel)
        {
            try
            {
                var result = await _userService.CreateUserAsync(viewModel);
                return StatusCode(201, result);
            }
            catch (MarketException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel viewModel)
        {
            try
            {
                var result = await _userService.LoginUserAsync(viewModel);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("refresh-user")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequstViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _userService.VerifyAndGenerateTokenAsync(viewModel);
            return Ok(result);
        }
    }
}
