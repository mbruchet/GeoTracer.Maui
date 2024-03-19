using GeoTracer.Api.Data;
using GeoTracer.Shared;
using GeoTracer.Shared.Messages;
using GeoTracer.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace GeoTracer.Api.EndPoints
{
    public static class AuthenticationEndPoint
    {
        public static IEndpointRouteBuilder RegisterAuthenticationEndPoint(this IEndpointRouteBuilder builder, IConfiguration configuration)
        {
            var group = builder.MapGroup("/api/authentication");

            group.MapPost("/Login", LoginUser)
            .WithName("LoginUser")
            .Produces<LoginResult>()
            .WithOpenApi();

            group.MapPost("/refreshToken", RefreshToken)
            .WithName("RefreshToken")
            .Produces<RefreshTokenResult>()
            .WithOpenApi();

            return builder;
        }

        private static async Task<IResult> LoginUser(LoginRequest request, ApplicationDbContext db)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.UserName, nameof(LoginRequest.UserName));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Password, nameof(LoginRequest.Password));

            var passwordHasher = new PasswordHasher();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if(user == null)
            {
                return Results.NotFound();
            }

            if (passwordHasher.VerifyPassword(request.Password, user.SaltPassword, user.Password))
            {
                var accessToken = new AccessToken
                {
                    AccessTokenId = Guid.NewGuid().ToString(),
                    AccessTokenValue = Guid.NewGuid().ToString(),
                    CreationDate = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(14),
                    UserName = user.UserName
                };

                await db.AccessTokens.AddAsync(accessToken); 
                
                return Results.Ok(new LoginResult
                {
                    Success = true,
                    AccessToken = accessToken,
                    AuthenticationType = "Remote",
                    User = user
                });
            }
            else
                return Results.BadRequest(new LoginResult { Success = false, Message = "Password" });
        }

        private static async Task<IResult> RefreshToken(RefreshTokenRequest request)
        {
            User user = new User(request.UserId)
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Email
            };

            user.SetRoles(new[] { "admin" });

            return Results.Ok(new RefreshTokenResult
            {
                AccessToken = new AccessToken
                {
                    AccessTokenId = request.AccessToken.AccessTokenId,
                    AccessTokenValue = request.AccessToken.AccessTokenValue,
                    ExpirationDate = DateTime.UtcNow.AddDays(14),
                    CreationDate= request.AccessToken.CreationDate,
                    UserName = request.AccessToken.UserName
                },
                Success = true,
                User = user
            });
        }
    }
}
