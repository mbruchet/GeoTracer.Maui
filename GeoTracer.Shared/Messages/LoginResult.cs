namespace GeoTracer.Shared.Messages;

public record LoginResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public User User { get; set; }
    public AccessToken AccessToken { get; set; }
    public string? AuthenticationType { get; set; }
}
