namespace BLL.Dtos.MessageDtos;

public class CheckOtpDto
{
    public int VerificationCode { get; set; }
    public string SessionKey { get; set; } = string.Empty;
}