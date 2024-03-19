namespace GeoTracer.Shared.Messages;

public record RefreshTokenResult
{
    public bool Success { get; set; }
    public User User { get; set; }
    public AccessToken AccessToken { get; set; }
    public string Message { get; set; }
}
