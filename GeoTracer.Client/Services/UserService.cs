using GeoTracer.Shared;
using GeoTracer.Shared.Messages;
using GeoTracer.Shared.Services;
using SQLite;

namespace GeoTracer.Client.Services;

public class UserService(RemoteUserService remoteUserService, PasswordHasher passwordHasher)
{
    private class UserDatabase
    {
        SQLiteAsyncConnection _connection;

        internal SQLiteAsyncConnection Connection => _connection;

        public UserDatabase()
        {

        }

        internal async Task Init()
        {
            if (_connection is not null)
                return;

            _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _connection.CreateTableAsync<User>();
            await _connection.CreateTableAsync<AccessToken>();
        }
    }

    UserDatabase db = new();

    public async Task<LoginResult> Login(string username, string password)
    {
        await db.Init();

        var users = await db.Connection.Table<User>().ToListAsync();
        var localUser = await db.Connection.Table<User>().FirstOrDefaultAsync(u => u.UserName == username);

        if (localUser == null)
        {
            var loginResult = await remoteUserService.Login(username, password);

            if (loginResult != null && loginResult.Success == true)
            {
                var user = loginResult.User;

                var hashedPassword = passwordHasher.HashPassword(password);

                user.Password = hashedPassword.Hash;
                user.SaltPassword = hashedPassword.Salt;

                await db.Connection.InsertAsync(user);
                await db.Connection.InsertAsync(loginResult.AccessToken);
                localUser = user;
            }

            return loginResult;
        }
        else
        {
            var accessToken = await db.Connection.Table<AccessToken>()
                .FirstOrDefaultAsync(t => t.UserName == username && t.CreationDate < DateTime.UtcNow &&
                t.ExpirationDate > DateTime.UtcNow);

            if (accessToken == null)
            {
                await db.Connection.DeleteAsync(localUser);

                var loginResult = await remoteUserService.Login(username, password);

                if (loginResult != null && loginResult.Success == true)
                {
                    await db.Connection.InsertAsync(loginResult.User);
                    await db.Connection.InsertAsync(loginResult.AccessToken);
                    localUser = loginResult.User;
                }

                return loginResult;
            }
            else if (!VerifyPassword(password, localUser))
            {
                return new LoginResult { Success = false, Message = "Password" };
            }
            else
            {
                if (accessToken.ExpirationDate > DateTime.UtcNow)
                {
                    return new LoginResult
                    {
                        Success = true,
                        AccessToken = accessToken,
                        User = await db.Connection.Table<User>().FirstOrDefaultAsync(u => u.UserName == username),
                        AuthenticationType = "RefreshToken"
                    };
                }
                else
                {
                    var refreshToken = await remoteUserService.RefreshToken(localUser, accessToken);

                    if (refreshToken != null && refreshToken.Success == true)
                    {
                        await db.Connection.UpdateAsync(refreshToken.AccessToken);

                        return new LoginResult
                        {
                            Success = true,
                            AccessToken = accessToken,
                            User = await db.Connection.Table<User>().FirstOrDefaultAsync(u => u.UserName == username),
                            AuthenticationType = "RefreshToken"
                        };
                    }
                    else
                    {
                        return new LoginResult { Success = false, Message = "RefreshTokenFailed" };
                    }
                }
            }

        }
    }

    private bool VerifyPassword(string enteredPassword, User localUser)
    {
        return passwordHasher.VerifyPassword(enteredPassword, localUser.SaltPassword, localUser.Password);
    }
}
