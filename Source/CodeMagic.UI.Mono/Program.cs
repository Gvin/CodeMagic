using System;
using CodeMagic.Core.Logging;
using CodeMagic.UI.Mono.GameProcess;

namespace CodeMagic.UI.Mono
{
    internal static class Program
    {
        public const int MapCellImageSize = 7;

        private static CodeMagicGame game;

        private static ILog log;

        [STAThread]
        internal static void Main()
        {
            try
            {
                GameConfigurator.Configure();

                log = LogManager.GetLog(nameof(Program));

                log.Info("Starting game");

                using (game = new CodeMagicGame())
                {
                    game.Run();
                }

                log.Info("Closing game");
            }
            catch (Exception exception)
            {
                log.Fatal(exception);
                throw;
            }
        }

        internal static void Exit()
        {
            game.Exit();
        }
    }
}
