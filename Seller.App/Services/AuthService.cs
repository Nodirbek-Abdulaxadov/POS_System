using BLL.Dtos.Identity;
using BLL.Dtos.MessageDtos;
using Newtonsoft.Json;
using Seller.App.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Seller.App.Services;

public class AuthService : IDisposable
{
    private string baseUrl = Constants.BASE_URL + "auth/";

    public void Dispose()
            => GC.SuppressFinalize(this);

    public async Task<(bool, string?)> LoginAsync(string phoneNumber, string password)
    {
        var data = new
        {
            phoneNumber = phoneNumber, 
            password = password 
        };

        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();
        using var response = await httpClient.PostAsync(baseUrl + "login", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<AuthResultViewModel>(responseContent);

            using var tokenService = new TokenService();
            tokenService.SaveCreditionals(loginResponse?? new AuthResultViewModel());

            return (true, "Successfully login!");
        }
        else
        {
            return (false, response.ReasonPhrase?.ToString());
        }
    }

    public async Task<bool> RefreshToken()
    {
        using var tokenService = new TokenService();
        var data = new
        {
            token = tokenService.GetToken(),
            refreshToken = tokenService.GetRefreshToken()
        };
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();
        using var response = await httpClient.PostAsync(baseUrl + "refresh-user", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<AuthResultViewModel>(responseContent);

            tokenService.SaveCreditionals(loginResponse ?? new AuthResultViewModel());

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<CheckOtpDto> SendOtp(string phoneNumber)
    {
        using var httpClient = new HttpClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        string deviceId = DeviceInfo.ReturnHardWareID().Result;
        using var response = await httpClient.PostAsync(Constants.BASE_URL + $"otp/send/{phoneNumber}/{deviceId}" , content);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var otpResponse = JsonConvert.DeserializeObject<CheckOtpDto>(responseContent);
            return otpResponse;
        }

        return null;
    }
}