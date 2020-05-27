namespace CodeMagic.Core.Logging
{
    public interface ILogProvider
    {
        ILog GetLog<T>();

        ILog GetLog(string context);
    }
}