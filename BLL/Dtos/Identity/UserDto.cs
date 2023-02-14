using Core;

namespace BLL.Dtos.Identity;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 
    public string PhoneNumber { get; set; } = string.Empty;

    public static explicit operator UserDto(User v)
        => new UserDto()
        {
            Id = v.Id,
            FullName = v.FullName,
            Email = v.Email??"",
            PhoneNumber = v.PhoneNumber??""
        };
}