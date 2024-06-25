using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Resolute.Errors;
using TaskHive.API.Providers;
using TaskHive.API.Services;
using TaskHive.API.Settings;
using TaskHive.API.Tests.MockSetup;
using TaskHive.API.Tests.TestData;

namespace TaskHive.API.Tests.ServiceTests;

public class AccountServiceTests
{
    readonly IUserManagerProvider userManagerProviderMock;
    readonly ILogger<AccountService> loggerMock;
    readonly AppSettings appSettings;


    public AccountServiceTests()
    {
        userManagerProviderMock = UserManagerProviderMockSetup.ValidUserManagerProvider();
        loggerMock = Substitute.For<ILogger<AccountService>>();
        appSettings = AppSettingsSetup.ValidAppSettings();
    }

    private AccountService CreateAccountService()
    {
        return new AccountService(appSettings, userManagerProviderMock, loggerMock);
    }

    [Fact]
    public async Task RegisterAsync_ValidUser_Succeeds()
    {
        var user = UserTestData.ValidUser();

        userManagerProviderMock.WithFindByEmailAsync(user.Email!, null!);

        var accountService = CreateAccountService();

        var result = await accountService.RegisterAsync(user, "password", "CreatedByUserId");

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RegisterAsync_EmailAlreadyTaken_ConflictResult()
    {
        var user = UserTestData.ValidUser();
        var existingUser = UserTestData.ValidUser();

        userManagerProviderMock.WithFindByEmailAsync(user.Email!, existingUser);

        var accountService = CreateAccountService();

        var result = await accountService.RegisterAsync(user, "password", "CreatedByUserId");

        Assert.True(result.Failed);

        Assert.Equal(ErrorCode.Conflict, result.Error.ErrorCode);
    }

    [Fact]
    public async Task LoginAsync_ValidUser_Succeeds()
    {
        var user = UserTestData.ValidUser();
        var password = "password";

        userManagerProviderMock
            .WithFindByEmailAsync(user.Email!, user)
            .WithCheckPasswordAsync(user, password, true)
            .WithGetRolesAsync(user, [nameof(RoleNames.User)]);

        var accountService = CreateAccountService();

        var result = await accountService.LoginAsync(user.Email!, password);

        Assert.True(result.Succeeded);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_NotFoundResult()
    {
        var user = UserTestData.ValidUser();
        var password = "password";

        userManagerProviderMock
            .WithFindByEmailAsync(user.Email!, null!);

        var accountService = CreateAccountService();

        var result = await accountService.LoginAsync(user.Email!, password);

        Assert.True(result.Failed);
        Assert.Equal(ErrorCode.NotFound, result.Error.ErrorCode);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_UnauthorizedResult()
    {
        var user = UserTestData.ValidUser();
        var password = "password";

        userManagerProviderMock
            .WithFindByEmailAsync(user.Email!, user)
            .WithCheckPasswordAsync(user, password, false);

        var accountService = CreateAccountService();

        var result = await accountService.LoginAsync(user.Email!, password);

        Assert.True(result.Failed);
        Assert.Equal(ErrorCode.Unauthorized, result.Error.ErrorCode);
    }
}