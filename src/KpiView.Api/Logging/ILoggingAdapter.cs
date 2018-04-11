using System;

namespace KpiView.Api.Logging
{

    public interface ILoggingAdapter
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(Exception ex, string message, params object[] args);
    }
    public interface ILoggingAdapter<T> : ILoggingAdapter
    {

    }
}