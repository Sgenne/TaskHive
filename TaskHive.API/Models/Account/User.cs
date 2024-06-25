using Microsoft.AspNetCore.Identity;

namespace TaskHive.API.Models;

public class User : IdentityUser
{
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string CreatedByUserId { get; set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ModifiedByUserId { get; set; } = string.Empty;
}