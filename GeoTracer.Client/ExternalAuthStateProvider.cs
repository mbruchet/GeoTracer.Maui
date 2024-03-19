namespace GeoTracer.Client;

using System.Security.Claims;
using System.Threading.Tasks;
using GeoTracer.Client.Services;
using GeoTracer.Shared.Messages;
using Microsoft.AspNetCore.Components.Authorization;

public class ExternalAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());
    private UserService _userService;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(currentUser));
    }

    public ExternalAuthStateProvider(UserService userService)
    {
        _userService = userService;
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var loginResult = await LoginWithExternalProviderAsync(username, password);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
        return loginResult;
    }

    private async Task<LoginResult> LoginWithExternalProviderAsync(string userName, string password)
    {
        var loginResult = await _userService.Login(userName, password);
        
        if(!loginResult.Success)
            return loginResult;

        var identity = new ClaimsIdentity(authenticationType: loginResult.AuthenticationType);

        var userId = loginResult.User.UserId;

        identity.AddClaims(
            [
                new Claim(ClaimTypes.Email, loginResult.User.Email),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, loginResult.User.Name)
            ]);

        foreach(var role in loginResult.User.Roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        currentUser = new ClaimsPrincipal(identity);
        return loginResult;
    }

    public Task Logout()
    {
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentUser)));

        return Task.CompletedTask;
    }
}