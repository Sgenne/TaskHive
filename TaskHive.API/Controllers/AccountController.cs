using Microsoft.AspNetCore.Mvc;
using Resolute.Errors;
using TaskHive.API.Extensions;
using TaskHive.API.Models;
using TaskHive.API.Requests.Account;
using TaskHive.API.Services;

namespace TaskHive.API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var user = new User()
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var registerResult = await accountService.RegisterAsync(user, request.Password, User.UserId());

        if (registerResult.Failed)
        {
            if (registerResult.Error.ErrorCode == ErrorCode.Conflict && 
                registerResult.Error.Source == nameof(user.Email) && 
                !string.IsNullOrWhiteSpace(registerResult.Error.SanitizedMessage))
            {
                return Conflict(registerResult.Error.SanitizedMessage);
            } 

            return StatusCode(500, "Something went wrong when registering the user.");
        }

        return Ok();
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loginResult = await accountService.LoginAsync(request.Email, request.Password);

        if (loginResult.Failed)
        {
            return BadRequest(loginResult.Error.SanitizedMessage);
        }

        var response = new LoginResponse(loginResult.Value);

        return Ok(response);
    }

}