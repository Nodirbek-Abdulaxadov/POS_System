using BLL.Dtos.Identity;
using Core;

namespace BLL.Interfaces;
public interface IUserService
{
    Task<bool> CheckInRole(LoginUserViewModel viewModel, string role);
    Task<UserDto> CreateUserAsync(RegisterUserViewModel viewModel);
    Task<string> LoginUserAsync(LoginUserViewModel viewModel);
    Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequstViewModel viewModel);
}