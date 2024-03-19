using SQLite;
using System.ComponentModel.DataAnnotations;

namespace GeoTracer.Shared;

public class AccessToken
{
    [Key, PrimaryKey]
    public string AccessTokenId { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreationDate { get; set; }
    public string AccessTokenValue { get; set; }
}
