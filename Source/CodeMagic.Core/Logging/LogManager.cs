namespace CodeMagic.Core.Logging
{
    public static class LogManager
    {
        private static ILogProvider logProvider;

        public static void Initialize(ILogProvider provider)
        {
            logProvider = provider;
        }

        public static ILog GetLog<T>()
        {
            return logProvider.GetLog<T>();
        }

        public static ILog GetLog(string context)
        {
            return logProvider.GetLog(context);
        }
    }
}