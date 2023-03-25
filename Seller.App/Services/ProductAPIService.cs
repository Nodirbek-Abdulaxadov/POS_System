using BLL.Dtos.ProductDtos;
using Newtonsoft.Json;
using Seller.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Seller.App.Services;

public class ProductAPIService : IDisposable
{
    HttpClient _client;
    TokenService tokenService;
    public ProductAPIService()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Constants.BASE_URL);
        tokenService = new TokenService();
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenService.GetToken());
    }

    public async Task<List<DProduct>?> GetProducts()
    {
        var response = _client.GetStringAsync("product/d");
        var res = response.Result;
        if (response != null)
        {
            return JsonConvert.DeserializeObject<List<DProduct>>(res);
        }
        

        return null;
    }

    public void Dispose()
        => GC.SuppressFinalize(this);
}