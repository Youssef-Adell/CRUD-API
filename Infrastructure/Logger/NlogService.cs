using Core.IServices;
using NLog;

namespace Infrastructure.Logger;
public class NlogService : ILoggerService
{
    private readonly ILogger _logger;

    public NlogService(ILogger logger)
    {
        _logger = logger;
    }

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogInfo(string message) => _logger.Info(message);

    public void LogWarn(string message) => _logger.Warn(message);
}
