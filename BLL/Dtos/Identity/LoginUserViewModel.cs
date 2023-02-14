using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.Identity;

public class LoginUserViewModel
{
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}