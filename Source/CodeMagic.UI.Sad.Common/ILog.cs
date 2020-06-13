using System;

namespace CodeMagic.UI.Sad.Common
{
    public interface ILog
    {
        void Debug(string message);

        void Debug(string message, Exception exception);

        void Debug(Exception exception);

        void Info(string message);

        void Info(string message, Exception exception);

        void Info(Exception exception);

        void Warning(string message);

        void Warning(string message, Exception exception);

        void Warning(Exception exception);

        void Error(string message);

        void Error(string message, Exception exception);

        void Error(Exception exception);

        void Fatal(string message);

        void Fatal(string message, Exception exception);

        void Fatal(Exception exception);
    }
}