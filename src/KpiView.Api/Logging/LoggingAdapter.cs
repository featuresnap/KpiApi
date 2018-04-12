using System;
using Microsoft.Extensions.Logging;

namespace KpiView.Api.Logging
{
    public class LoggingAdapter<T> : ILoggingAdapter<T>
    {
        private ILogger<T> _logger;

        public LoggingAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }
    }
}