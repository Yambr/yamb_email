using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yambr.Email.SDK.Extensions
{
    public static class LoggerExtensions
    {
        public static void Info(this ILogger logger,  string message)
        {
            logger.LogInformation(message);
        }
    }
}
