using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GeoTracer.Client.Services;
using GeoTracer.Shared.Services;

namespace GeoTracer.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Roboto-Light.ttf", "Roboto-Light");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddMudServices();
            builder.Services.AddBlazorWebView();

            builder.Services.AddAuthorizationCore();

            builder.Services.TryAddTransient<RemoteUserService>();
            builder.Services.TryAddTransient<UserService>();
            builder.Services.TryAddSingleton<AuthenticationStateProvider, ExternalAuthStateProvider>();
            builder.Services.TryAddSingleton<ApplicationSettings>();
            builder.Services.TryAddSingleton<PasswordHasher>();

            builder.Services.AddCascadingAuthenticationState();

            var host = builder.Build();
            return host;
        }
    }
}
