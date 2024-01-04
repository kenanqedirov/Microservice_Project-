using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Services.Interfaces;
using FreeCourse.Web.Models;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Services
{
    //public class IdentityService : IIdentityService
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly IHttpContextAccessor _httContextAccessor;
    //    private readonly ClientSettings _clientSettings;
    //    private readonly ServiceApiSettings _serviceApiSettings;

    //    public IdentityService(HttpClient client, IHttpContextAccessor httpontextAccessor,
    //        IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
    //    {
    //        _httpClient = client;
    //        _httContextAccessor = httpontextAccessor;
    //        _clientSettings = clientSettings.Value;
    //        _serviceApiSettings = serviceApiSettings.Value;
    //    }

    //    public Task<TokenResponse> GetAccessByRefreshToken()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Task RevokeRefreshToken()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public async Task<Response<bool>> SignIn(SigninInput signinInput)
    //    {
    //        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
    //        {
    //            Address = _serviceApiSettings.BaseUri,
    //            Policy = new DiscoveryPolicy {RequireHttps = false}
    //        });
    //        if (disco.IsError)
    //        {
    //            throw disco.Exception;
    //        }
    //        var passwordTokenRequest = new PasswordTokenRequest
    //        {
    //            ClientId = _clientSettings.WebClientForUser.ClientId,
    //            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
    //            UserName = signinInput.Email,
    //            Password = signinInput.Password,
    //            Address= disco.TokenEndpoint
    //        };
    //        var token =await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
    //        if (token.IsError)
    //        {
    //            var responseToken = await token.HttpResponse.Content.ReadAsStringAsync();
    //        }
    //    }
    //}
}
