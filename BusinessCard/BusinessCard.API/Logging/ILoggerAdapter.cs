namespace BusinessCard.API.Logging;

public interface ILoggerAdapter<T>
{
    void LogInformation(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogCritical(string message, params object[] args);
}

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message,args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogCritical(string message, params object[] args)
    {
        _logger.LogCritical(message, args);
    }
}
