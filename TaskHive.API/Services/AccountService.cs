using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Resolute.Results;
using TaskHive.API.Providers;
using TaskHive.API.Settings;
using Resolute.Errors;
using TaskHive.API.Models;
using TaskHive.API.Extensions;

namespace TaskHive.API.Services;

public interface IAccountService
{
    Task<Result<User>> RegisterAsync(User user, string password, string createdByUserId);
    Task<Result<string>> LoginAsync(string email, string password);

    Task<string> GenerateTokenAsync(User user);
    Task<ClaimsIdentity> GenerateClaimsIdentityAsync(User user);
}

public class AccountService(AppSettings appSettings, IUserManagerProvider userManagerProvider, ILogger<AccountService> logger) : IAccountService
{

    public async Task<Result<User>> RegisterAsync(User user, string password, string createdByUserId)
    {
        var logParams = new Dictionary<string, object>()
        {
            [nameof(createdByUserId)] = createdByUserId,
        };

        logger.LogEntering(nameof(AccountService), nameof(RegisterAsync), logParams);

        if (string.IsNullOrEmpty(user.Email))
        {
            return Result.Failure<User>("No email was provided.", ErrorCode.BadRequest);
        }

        User? existingUserWithEmail = await userManagerProvider.FindByEmailAsync(user.Email);

        if (existingUserWithEmail != null)
        {
            return Result.Failure<User>("Email already taken", ErrorCode.Conflict, "The provided email was already taken.", nameof(user.Email));
        }

        user.CreatedByUserId = createdByUserId;
        user.ModifiedByUserId = createdByUserId;

        var identityResult = await userManagerProvider.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            var errorMessages = identityResult.Errors.Select(error => error.Description);
            logger.LogErrorMessages(nameof(AccountService), nameof(RegisterAsync), logParams, errorMessages);

            return Result.Failure<User>("Failed to register user", ErrorCode.InternalServerError, "Something went wrong while trying to register the user.");
        }

        logParams.Add("CreatedUserId", user.Id);
        logger.LogExiting(nameof(AccountService), nameof(RegisterAsync), logParams);

        return Result.Success(user);
    }

    public async Task<Result<string>> LoginAsync(string email, string password)
    {
        logger.LogEntering(nameof(AccountService), nameof(LoginAsync), []);

        var sanitizedErrorMessage = "The provided credentials were not valid.";

        var user = await userManagerProvider.FindByEmailAsync(email);

        if (user == null)
        {
            return Result.Failure<string>("No user with the provided email was found.", ErrorCode.NotFound, sanitizedErrorMessage, nameof(email));
        }

        var passwordIsValid = await userManagerProvider.CheckPasswordAsync(user, password);

        if (!passwordIsValid)
        {
            return Result.Failure<string>("The provided password was not valid", ErrorCode.Unauthorized, sanitizedErrorMessage, nameof(password));
        }

        var token = await GenerateTokenAsync(user);

        return token.ToResult();
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var secret = Encoding.ASCII.GetBytes(appSettings.JWTSettings.Secret);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secret),
            SecurityAlgorithms.HmacSha512Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = await GenerateClaimsIdentityAsync(user),
            Expires = DateTime.UtcNow.AddMinutes(appSettings.JWTSettings.ExpirationInMinutes),
            SigningCredentials = signingCredentials,
            Issuer = appSettings.JWTSettings.Issuer,
            Audience = appSettings.JWTSettings.Audience
        };

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    public async Task<ClaimsIdentity> GenerateClaimsIdentityAsync(User user)
    {
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ArgumentException("User name is required");
        }

        var roleNames = await userManagerProvider.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var roleClaims = roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName));
        claims.AddRange(roleClaims);

        return new ClaimsIdentity(claims);
    }
}
