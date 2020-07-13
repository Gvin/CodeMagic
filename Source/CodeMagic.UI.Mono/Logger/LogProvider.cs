using System;
using CodeMagic.Core.Logging;
using Serilog;
using Serilog.Events;

namespace CodeMagic.UI.Mono.Logger
{
    internal class LogProvider : ILogProvider
    {
        private const string LogFilePath = @".\log_.txt";
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;

        private readonly ILogger logger;

        public LogProvider()
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(LogFilePath, GetLogLevel(), 
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}", 
                    rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 5242880, retainedFileCountLimit: 5)
                .CreateLogger();
        }

        private static LogEventLevel GetLogLevel()
        {
            var stringValue = Settings.Current.LogLevel;
            if (Enum.TryParse(stringValue, true, out LogEventLevel level))
            {
                return level;
            }

            return DefaultLogLevel;
        }

        public ILog GetLog<T>()
        {
            return GetLog(typeof(T).Name);
        }

        public ILog GetLog(string context)
        {
            return new SerilogLog(logger.ForContext("Context", context));
        }
    }
}