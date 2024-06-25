using System.ComponentModel.DataAnnotations;

namespace TaskHive.API.Requests.Account;

public class RegisterUserRequest
{
    [MinLength(1)]
    [MaxLength(128)]
    public string UserName { get; set; } = string.Empty;
    
    [EmailAddress]
    [MaxLength(128)]
    public string Email { get; set; } = string.Empty;
    
    [MinLength(8)]
    [MaxLength(1024)]
    public string Password { get; set; } = string.Empty;
}