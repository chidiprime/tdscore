using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Core.Interfaces;

namespace TDS.Service.Implementation
{
    public class Log:ILog
    {
        private readonly ILoggerFactory _loggerFactory;

        public Log(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void Error(Exception ex)
        {
            var logger = _loggerFactory.CreateLogger("Error Log");
            logger.LogError($"Message: {ex.Message}");
            logger.LogError($"Inner Exception: {ex.InnerException?.Message}");
        }

        public void Info(object data, string message)
        {
            var logger = _loggerFactory.CreateLogger("Info");
            logger.LogInformation(message, data);
        }

        public void Write(string msg)
        {
            var logger = _loggerFactory.CreateLogger("Write");
            logger.LogInformation(msg);
        }
    }
}
