using System;
using Microsoft.Extensions.Logging;

namespace Yambr.SDK.Extensions
{
    public static class LoggerExtensions
    {
        public static void Info(this ILogger logger,  string message)
        {
            logger.LogInformation(message);
        }

        public static void Debug(this ILogger logger, string message)
        {
            logger.LogDebug(message);
        }
        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            logger.LogDebug(message, args);
        }

        public static void Error(this ILogger logger, Exception exception, string message)
        {
            logger.LogError(exception, message);
        }

        public static void Error(this ILogger logger, Exception exception, string message, params object[] args)
        {
            logger.LogError(exception, message, args);
        }
    }
}
