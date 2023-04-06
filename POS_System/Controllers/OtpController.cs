using BLL.Dtos.MessageDtos;
using BLL.Interfaces;
using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OtpController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly UserManager<User> _userManager;

    public OtpController(IMessageService messageService,
                         UserManager<User> userManager)
	{
        _messageService = messageService;
        _userManager = userManager;
    }

    [HttpPost("send/{phoneNumber}/{deviceInfo}")]
    public async Task<IActionResult> Send(string phoneNumber, string deviceInfo)
    {
        if (string.IsNullOrEmpty(phoneNumber) ||
            string.IsNullOrEmpty(deviceInfo))
        {
            return BadRequest();
        }

        if (!PhoneNumberIsExist(phoneNumber))
        {
            return NotFound();
        }

        try
        {
            var request = await _messageService.SendOTP(phoneNumber, deviceInfo);
            return Ok(request);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("check")]
    public async Task<ActionResult<bool>> Check([FromBody] CheckOtpDto otpDto)
    {
        if (otpDto == null)
        {
            return BadRequest();
        }

        var result = await _messageService.CheckOTP(otpDto);
        return Ok(result);
    }

    private bool PhoneNumberIsExist(string phoneNumber)
    {
        var users = _userManager.Users.ToList();
        return users.Any(x => x.PhoneNumber == phoneNumber);
    }
}