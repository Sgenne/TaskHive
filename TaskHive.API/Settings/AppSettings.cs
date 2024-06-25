namespace TaskHive.API.Settings;

public class AppSettings
{
    public JWTSettings JWTSettings { get; init; } = new JWTSettings();
    public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
}

public class ConnectionStrings
{
    public string Application { get; set; } = string.Empty;
}

public class JWTSettings
{
    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpirationInMinutes { get; init; }
}