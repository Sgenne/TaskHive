using Microsoft.Extensions.Configuration;
using TaskHive.API.Settings;

namespace TaskHive.API.Tests.MockSetup;

public static class AppSettingsSetup
{
    public static AppSettings ValidAppSettings()
    {
        var appSettings = new AppSettings() {
            JWTSettings = new JWTSettings() {
                Audience = "Audience",
                ExpirationInMinutes = 10,
                Issuer = "Issuer",
                Secret = "WOW SUCH A SUPER SECRET SECRET!!! OH MY GOD IT IS SO SECRET!!! WOW!!!"
            }
        };

        return appSettings;
    }
}