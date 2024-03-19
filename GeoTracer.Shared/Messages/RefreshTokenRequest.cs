namespace GeoTracer.Shared.Messages;

public class RefreshTokenRequest
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public AccessToken AccessToken { get; set; }
}
