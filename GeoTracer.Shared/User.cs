using SQLite;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace GeoTracer.Shared;

public record User()
{
    public User(string userId):this()
	{
		UserId = userId ?? Guid.NewGuid().ToString();
	}

	public User(string userId, string userName):this(userId)
	{
		UserName = userName;
	}
	[Key, PrimaryKey]
    public string UserId { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
    public string Name { get; set; }

	[Ignore]
	public string[] Roles { get; private set; }

	public void SetRoles(string[] roles)
	{
		Roles = roles;
	}

	public string JsRoles
	{
		get
		{
			if(Roles?.Count() >= 1)
				return String.Join(',', Roles);

			return string.Empty;
		}
		set
		{
			if(!string.IsNullOrEmpty(value))
				Roles = value.Split(',');
		}
	}

	[JsonIgnore]
	public string Password { get; set; }
	[JsonIgnore]
	public string SaltPassword { get; set; }
}
