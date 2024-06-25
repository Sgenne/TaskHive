namespace TaskHive.API.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        return str[0].ToString().ToUpper() + str.Substring(1);
    }
}
