using System;
using CodeMagic.UI.Sad.Common;

namespace SymbolsImageEditor
{
    public class NullLog : ILog
    {
        public void Debug(string message)
        {
        }

        public void Debug(string message, Exception exception)
        {
        }

        public void Debug(Exception exception)
        {
        }

        public void Info(string message)
        {
        }

        public void Info(string message, Exception exception)
        { 
        }

        public void Info(Exception exception)
        {
        }

        public void Warning(string message)
        {
        }

        public void Warning(string message, Exception exception)
        {
        }

        public void Warning(Exception exception)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception exception)
        {
        }

        public void Error(Exception exception)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, Exception exception)
        {
        }

        public void Fatal(Exception exception)
        {
        }
    }
}