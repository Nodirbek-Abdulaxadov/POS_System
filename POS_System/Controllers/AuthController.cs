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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService,
                              ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserViewModel viewModel)
        {
            try
            {
                var result = await _userService.CreateUserAsync(viewModel);
                _logger.LogInformation($"New user created: \"{viewModel.FullName}\"");
                return StatusCode(201, result);
            }
            catch (MarketException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel viewModel)
        {
            try
            {
                var result = await _userService.LoginUserAsync(viewModel);
                _logger.LogInformation($"User login: \"{viewModel.PhoneNumber}\"");
                return Ok(result);
            }
            catch (MarketException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("refresh-user")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequstViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Given model state invalid");
                    return BadRequest();
                }
                var result = await _userService.VerifyAndGenerateTokenAsync(viewModel);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }
        }
    }
}
