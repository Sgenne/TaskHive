

using TaskHive.API.Models;

namespace TaskHive.API.Tests.TestData;

public static class UserTestData
{
    public static User ValidUser(User? fromUser = null)
    {
        if (fromUser == null)
        {
            fromUser = new User();
        }

        var user = new User()
        {
            Id = !string.IsNullOrEmpty(fromUser.Id) ? fromUser.Id : "1",
            UserName = !string.IsNullOrEmpty(fromUser.UserName) ? fromUser.UserName : "UserName",
            Email = !string.IsNullOrEmpty(fromUser.Email) ? fromUser.Email : "abc@test.com",
            PhoneNumber = !string.IsNullOrEmpty(fromUser.PhoneNumber) ? fromUser.PhoneNumber : "0702276538",
            PasswordHash = !string.IsNullOrEmpty(fromUser.PasswordHash) ? fromUser.PasswordHash : "$2y$10$jFpt7BEDQPj./usWnykg7u3UyoPQ4pymFx8/k4hbjQVCm0tyYTkUe",
            SecurityStamp = !string.IsNullOrEmpty(fromUser.SecurityStamp) ? fromUser.SecurityStamp : "b0b6001f-a8f7-4d6f-8174-2ae8a7a41163",
            ConcurrencyStamp = !string.IsNullOrEmpty(fromUser.ConcurrencyStamp) ? fromUser.ConcurrencyStamp : "a1a6001f-a8f7-4d6f-8174-2ae8a7a41163",
            NormalizedUserName = !string.IsNullOrEmpty(fromUser.NormalizedUserName) ? fromUser.NormalizedUserName : "USERNAME",
            NormalizedEmail = !string.IsNullOrEmpty(fromUser.NormalizedEmail) ? fromUser.NormalizedEmail : "ABC@TEST.COM",
            TwoFactorEnabled = fromUser.TwoFactorEnabled,
            LockoutEnd = fromUser.LockoutEnd,
            LockoutEnabled = fromUser.LockoutEnabled,
            AccessFailedCount = fromUser.AccessFailedCount
        };

        return user;
    }
}