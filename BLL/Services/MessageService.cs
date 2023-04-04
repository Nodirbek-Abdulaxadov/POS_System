using BLL.Dtos.MessageDtos;
using BLL.Interfaces;
using BLL.Validations;
using Messager;
using static System.Net.WebRequestMethods;

namespace BLL.Services;

public class MessageService : IMessageService
{
    public Task<bool> CheckOTP(CheckOtpDto otp)
    {
        if (otp == null || 
            string.IsNullOrEmpty(otp.SessionKey) ||
            otp.VerificationCode < 0)
        {
            return Task.FromResult(false);
        }

        var sessionKey = $"{otp.SessionKey.Split('|')[0]}|{otp.VerificationCode}";
        var isValid = sessionKey.Equals(otp.SessionKey);

        return Task.FromResult(isValid);
    }

    public async Task<CheckOtpDto> SendOTP(string phoneNumber, string deviceInfo)
    {
        using var messager = new Message();
        var request = await messager.SendSMSAsync(phoneNumber);
        if (request.Success)
        {
            CheckOtpDto otp = new()
            {
                VerificationCode = request.Code,
                SessionKey = $"{deviceInfo}|{request.Code.GetHashCode()}"
            };

            return otp;
        }
        else
        {
            throw new MarketException(request.Message);
        }
    }
}