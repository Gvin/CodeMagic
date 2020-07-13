using System;
using Serilog;
using ICoreLog = CodeMagic.Core.Logging.ILog;

namespace CodeMagic.UI.Mono.Logger
{
    internal class SerilogLog : ICoreLog
    {
        private readonly ILogger logger;

        public SerilogLog(ILogger logger)
        {
            this.logger = logger;
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            logger.Debug(exception, message);
        }

        public void Debug(Exception exception)
        {
            logger.Debug(exception, "Error");
        }

        public void Info(string message)
        {
            logger.Information(message);
        }

        public void Info(string message, Exception exception)
        {
            logger.Information(exception, message);
        }

        public void Info(Exception exception)
        {
            logger.Information(exception, "Error");
        }

        public void Warning(string message)
        {
            logger.Warning(message);
        }

        public void Warning(string message, Exception exception)
        {
            logger.Warning(exception, message);
        }

        public void Warning(Exception exception)
        {
            logger.Warning(exception, "Error");
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(exception, message);
        }

        public void Error(Exception exception)
        {
            logger.Error(exception, "Error");
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            logger.Fatal(exception, message);
        }

        public void Fatal(Exception exception)
        {
            logger.Fatal(exception, "Fatal error");
        }
    }
}