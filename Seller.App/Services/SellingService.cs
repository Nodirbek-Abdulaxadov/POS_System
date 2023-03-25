using BLL.Dtos.ReceiptDtos;
using Seller.App.Models;
using System;
using System.Net.Http;

namespace Seller.App.Services;

public class SellingService : IDisposable
{
    HttpClient _client;
    TokenService tokenService;
    public SellingService()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Constants.BASE_URL + "receipt/");
        tokenService = new TokenService();
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenService.GetToken());
    }

    public ReceiptDto CreateEmptyReceipt()
        => new ReceiptDto();

    public void Dispose()
            => GC.SuppressFinalize(this);
}