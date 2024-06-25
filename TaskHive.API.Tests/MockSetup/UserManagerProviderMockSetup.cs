using Microsoft.AspNetCore.Identity;
using NSubstitute;
using TaskHive.API.Models;
using TaskHive.API.Providers;
using TaskHive.API.Tests.TestData;

namespace TaskHive.API.Tests.MockSetup;

public static class UserManagerProviderMockSetup
{
    public static IUserManagerProvider ValidUserManagerProvider()
    {
        var userManagerProvider = Substitute.For<IUserManagerProvider>();

        var validUser = UserTestData.ValidUser();

        userManagerProvider
            .WithFindByEmailAsync(validUser.Email!, validUser)
            .WithCreateAsync(IdentityResult.Success);

        return userManagerProvider;
    }

    public static IUserManagerProvider WithFindByEmailAsync(this IUserManagerProvider userManagerProvider, string emailArg, User? result)
    {
        userManagerProvider.FindByEmailAsync(emailArg)
            .Returns(result);

        return userManagerProvider;
    }
    public static IUserManagerProvider WithCreateAsync(this IUserManagerProvider userManagerProvider, IdentityResult result)
    {
        userManagerProvider.CreateAsync(Arg.Any<User>(), Arg.Any<string>())
            .Returns(result);

        return userManagerProvider;
    }

    public static IUserManagerProvider WithCheckPasswordAsync(this IUserManagerProvider userManagerProvider, User user, string password, bool result)
    {
        userManagerProvider.CheckPasswordAsync(user, password)
            .Returns(result);

        return userManagerProvider;
    }

    public static IUserManagerProvider WithGetRolesAsync(this IUserManagerProvider userManagerProvider, User user, List<string> roles)
    {
        userManagerProvider.GetRolesAsync(user)
            .Returns(roles);

        return userManagerProvider;
    }
}