namespace TaskHive.API.Extensions;

public static class LoggerExtensions
{
    public static void LogEntering<T>(this ILogger<T> logger, string className, string methodName, Dictionary<string, object> parameters)
    {
        var paramsString = string.Join(", ", parameters.Select(kvp => kvp.Key.Capitalize() + ": {" + kvp.Key.Capitalize() + "}"));
        var paramsArray = AddBaseParameters(className, methodName, parameters);

        logger.LogDebug("Entering {Class}.{Method}(" + paramsString + ")", paramsArray);
    }

    public static void LogExiting<T>(this ILogger<T> logger, string className, string methodName, Dictionary<string, object> parameters, string? message = null)
    {
        var paramsString = string.Join(", ", parameters.Select(kvp => kvp.Key.Capitalize() + ": {" + kvp.Key.Capitalize() + "}"));
        var paramsArray = AddBaseParameters(className, methodName, parameters);

        logger.LogDebug("Exiting {Class}.{Method}(" + paramsString + ") " + (!string.IsNullOrEmpty(message) ? message : ""), paramsArray);
    }

    public static void LogDebug<T>(this ILogger<T> logger, string className, string methodName, Dictionary<string, object> parameters, string message)
    {
        var paramsString = string.Join(", ", parameters.Select(kvp => kvp.Key.Capitalize() + ": {" + kvp.Key.Capitalize() + "}"));
        var paramsArray = AddBaseParameters(className, methodName, parameters);

        logger.LogDebug("{Class}.{Method}(" + paramsString + ") " + message, paramsArray);
    }

    public static void LogErrorMessages<T>(this ILogger<T> logger, string className, string methodName, Dictionary<string, object> parameters, IEnumerable<string> errorMessages)
    {
        var paramsString = string.Join(", ", parameters.Select(kvp => kvp.Key.Capitalize() + ": {" + kvp.Key.Capitalize() + "}"));

        var paramsArray = AddBaseParameters(className, methodName, parameters);
        paramsArray.Append(errorMessages);

        errorMessages = errorMessages.Select(m => $"\"{m}\"");

        logger.LogError("{Class}.{Method}(" + paramsString + ") ErrorMessages: [{ErrorMessages}]", [.. paramsArray, errorMessages]);
    }

    private static object[] AddBaseParameters(string className, string methodName, Dictionary<string, object> parameters)
    {
        var baseParams = new List<object>() { className, methodName };
        baseParams.AddRange(parameters.Select(kvp => kvp.Value.ToString() ?? ""));

        return baseParams.ToArray();
    }
}