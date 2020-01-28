using System;
using System.Diagnostics;
using System.IO;

namespace CodeMagic.Core.Common
{
    public static class PerformanceMeter
    {
        private static string outputFile;

        private static bool Initialized => !string.IsNullOrEmpty(outputFile);

        public static void Initialize(string outputFilePath)
        {
            outputFile = outputFilePath;

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            File.Create(outputFile).Close();
        }

        public static IPerformanceCounter Start(string methodName)
        {
            return new PerformanceCounter(time =>
            {
                WriteTime(methodName, time);
            });
        }

        private static void WriteTime(string methodName, long time)
        {
            if (Initialized)
            {
                File.AppendAllText(outputFile, $"{methodName}: {time}\r\n");
            }
        }

        public interface IPerformanceCounter : IDisposable
        {
        }

        private class PerformanceCounter : IPerformanceCounter
        {
            private readonly Stopwatch stopwatch;
            private readonly Action<long> callback;

            public PerformanceCounter(Action<long> callback)
            {
                this.callback = callback;
                stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                stopwatch.Stop();
                callback?.Invoke(stopwatch.ElapsedMilliseconds);
            }
        }
    }
}