namespace GeoTracer.Shared.Messages;

public record LoginRequest()
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginRequest(string userName, string password):this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        UserName = userName;
        Password = password;
    }
}
