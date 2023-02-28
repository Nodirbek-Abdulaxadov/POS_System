using BLL.Dtos.Identity;
using BLL.Interfaces;
using BLL.Validations;
using Core;
using DataLayer.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private readonly TokenValidationParameters _validationParameters;

    public UserService(UserManager<User> userManager,
                       RoleManager<IdentityRole> roleManager,
                       IConfiguration configuration,
                       AppDbContext dbContext,
                       TokenValidationParameters validationParameters)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _dbContext = dbContext;
        _validationParameters = validationParameters;
    }
    public async Task<UserDto> CreateUserAsync(RegisterUserViewModel viewModel)
    {
        var userExist = await FindUserByPhoneNumberAsync(viewModel.PhoneNumber);
        if (userExist != null)
        {
            throw new MarketException("This phone number is already exist!");
        }

        User user = new()
        {
            FullName = viewModel.FullName,
            Email = viewModel.Email,
            PhoneNumber = viewModel.PhoneNumber,
            UserName = viewModel.FullName.Replace(" ", ""),
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (result.Succeeded)
        {
            switch (viewModel.UserRole.ToUpper())
            {
                case UserRoles.SuperAdmin:
                    await _userManager.AddToRoleAsync(user, UserRoles.SuperAdmin); break;
                case UserRoles.Admin:
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin); break;
                case UserRoles.Seller:
                    await _userManager.AddToRoleAsync(user, UserRoles.Seller); break;
            }

            return (UserDto)user;
        }

        throw new Exception(result.Errors.First().Description);
    }

    public async Task<string> LoginUserAsync(LoginUserViewModel viewModel)
    {
        var userExist = await FindUserByPhoneNumberAsync(viewModel.PhoneNumber);
        if (userExist != null && await _userManager.CheckPasswordAsync(userExist, viewModel.Password))
        {
            var token = _dbContext.RefreshTokens.FirstOrDefault(r => r.UserId == Guid.Parse(userExist.Id));
            if (token != null)
            {
                _dbContext.RefreshTokens.Remove(token);
                _dbContext.SaveChanges();
            }

            return JsonConvert.SerializeObject(await GenerateTokenAsync(userExist, null));
        }

        throw new MarketException("Login failed! Incorrect phone number or password!");
    }

    private async Task<AuthResultViewModel> GenerateTokenAsync(User user, RefreshToken refresh)
    {
        var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName??""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Sub, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:securityKey"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audence"],
            expires: DateTime.Now.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256));

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        if (refresh != null)
        {
            var rToken = new AuthResultViewModel()
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Token = jwtToken,
                RefreshToken = refresh.Token,
                ExpiresAt = token.ValidTo,
            };
            return rToken;
        }

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = Guid.Parse(user.Id),
            DateAdded = DateTime.UtcNow.ToString(),
            DataExpire = DateTime.UtcNow.AddMonths(1).ToString(),
            Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        var response = new AuthResultViewModel()
        {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = token.ValidTo,
        };

        return response;
    }

    public async Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequstViewModel viewModel)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var storedToken = _dbContext.RefreshTokens.FirstOrDefault(i => i.Token == viewModel.RefreshToken);

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

        try
        {
            var tokenCheckResult = tokenHandler.ValidateToken(viewModel.Token,
                                                              _validationParameters,
                                                              out var validatedToken);
            return await GenerateTokenAsync(user, storedToken);
        }
        catch (SecurityTokenExpiredException)
        {
            if (DateTime.Parse(storedToken.DataExpire) >= DateTime.UtcNow)
            {
                return await GenerateTokenAsync(user, storedToken);
            }
            else
            {
                return await GenerateTokenAsync(user, null);
            }
        }
    }

    public async Task<bool> CheckInRole(LoginUserViewModel viewModel, string role)
    {
        var user = await FindUserByPhoneNumberAsync(viewModel.PhoneNumber);
        if (user == null)
        {
            throw new MarketException("User not found!");
        }

        return await _userManager.IsInRoleAsync(user, role);
    }

    private async Task<User?> FindUserByPhoneNumberAsync(string phoneNumber)
    {
        var user = await _userManager.Users
                                     .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        return user;
    }
}