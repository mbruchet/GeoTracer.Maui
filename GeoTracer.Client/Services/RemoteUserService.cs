
using GeoTracer.Shared;
using GeoTracer.Shared.Messages;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GeoTracer.Client.Services;

public class RemoteUserService
{
    private ApplicationSettings _appSettings;
    private readonly ILogger<RemoteUserService> _logger;

    public RemoteUserService(ApplicationSettings settings, ILogger<RemoteUserService> logger)
    {
        _appSettings = settings;
        _logger = logger;
    }

    public async Task<LoginResult> Login(string username, string password)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            return await RemoteLogin(username, password);
        }
        else
        {
            return new LoginResult { Success = false, Message = "InternetConnectivity" };
        }
    }

    public async Task<RefreshTokenResult> RefreshToken(User user, AccessToken accessToken)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            return await RemoteRefreshToken(user, accessToken);
        else if (accessToken.ExpirationDate > DateTime.UtcNow)
            return new RefreshTokenResult { Success = true, User = user };

        return new RefreshTokenResult { Success = false };
    }

    private async Task<RefreshTokenResult> RemoteRefreshToken(User user, AccessToken accessToken)
    {
        var options = new RestClientOptions(_appSettings.ApiRemoteUrl.TrimEnd('/'));

        using var client = new RestClient(options);

        var request = new RestRequest("authentication/refreshToken");
        request.AddOrUpdateHeader("Accept", "application/json");

        _logger.LogInformation($"start RemoteRefreshToken {user.Email}");

        request.AddJsonBody(new RefreshTokenRequest
        {
            UserId = user.UserId, 
            Email = user.Email,
            AccessToken = accessToken,
        });

        var response = await client.PostAsync<RefreshTokenResult>(request);

        _logger.LogInformation($"result from server for RemoteRefreshToken" +
            $" {JsonSerializer.Serialize(response)}");

        return response;
    }

    private async Task<LoginResult> RemoteLogin(string username, string password)
    {
        var options = new RestClientOptions(_appSettings.ApiRemoteUrl.TrimEnd('/'));

        using var client = new RestClient(options);
        
        var request = new RestRequest("authentication/Login");
        request.AddOrUpdateHeader("Accept",  "application/json");

        _logger.LogInformation($"start RemoteLogin {username}");
        request.AddJsonBody(new LoginRequest(username, password));

        var response = await client.PostAsync<LoginResult>(request);

        _logger.LogInformation($"result from server for RemoteLogin {JsonSerializer.Serialize(response)}");

        return response;
    }
}
