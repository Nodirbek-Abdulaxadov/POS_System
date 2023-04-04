using BLL.Dtos.MessageDtos;

namespace BLL.Interfaces;

public interface IMessageService
{
    Task<CheckOtpDto> SendOTP(string phoneNumber, string deviceInfo);
    Task<bool> CheckOTP(CheckOtpDto otp);
} 