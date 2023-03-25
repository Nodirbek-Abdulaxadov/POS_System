using BLL.Dtos.Identity;
using BLL.Dtos.ReceiptDtos;
using Newtonsoft.Json;
using Seller.App.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Seller.App.Services;

public class SellingService : IDisposable
{
    HttpClient _client;
    TokenService tokenService;
    public SellingService()
    {
        _client = new HttpClient();
        //_client.BaseAddress = new Uri(Constants.BASE_URL + "receipt/");
        tokenService = new TokenService();
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenService.GetToken());
    }

    public async Task<ReceiptDto> AddAsync(AddReceiptDto dto)
    {
        var json = JsonConvert.SerializeObject(dto.Transactions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();
        using var response = await httpClient.PostAsync(
            $"{Constants.BASE_URL}Receipt??CreatedDate={dto.CreatedDate}&TotalPrice={dto.TotalPrice}&Discount={dto.Discount}&PaidCash={dto.PaidCash}&PaidCard={dto.PaidCard}&HasLoan={dto.HasLoan}&SellerId={dto.SellerId}", 
            content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var receipt = JsonConvert.DeserializeObject<ReceiptDto>(responseContent);

            return receipt;
        }
        else
        {
            return null;
        }
    }

    public AddReceiptDto CreateEmptyReceipt()
        => new AddReceiptDto();

    public void Dispose()
            => GC.SuppressFinalize(this);
}