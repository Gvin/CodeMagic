using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.Saving;
using CodeMagic.UI.Sad.Views;
using ILog = CodeMagic.Core.Logging.ILog;

namespace CodeMagic.UI.Sad
{
    internal static class Program
    {
        private static ILog log;

        public const int MapCellImageSize = 7;

        private class GameExitException : Exception
        {
            public GameExitException()
                : base("Game exit")
            {
            }
        }

        [STAThread]
        internal static void Main()
        {
            try
            {
                GameConfigurator.Configure();
                log = LogManager.GetLog(nameof(Program));

                var gameWidth = FontProvider.GetScreenWidth(FontTarget.Game);
                var gameHeight = FontProvider.GetScreenHeight(FontTarget.Game) / 2;
                SadConsole.Game.Create(gameWidth, gameHeight);

                SadConsole.Game.OnInitialize = Init;

                SadConsole.Game.Instance.Run();

                SadConsole.Game.Instance.Dispose();

                log.Info("Closing game");

                ImagesStorage.Dispose();
                CurrentGame.Load(null);

                GC.Collect();
                
                throw new GameExitException();
            }
            catch (GameExitException)
            {
                throw;
            }
            catch (Exception e)
            {
                log.Fatal(e);
                throw;
            }
        }

        private static void Init()
        {
            SadConsole.Game.Instance.Window.Title = "C0de Mag1c";
            SadConsole.Game.Instance.Window.AllowUserResizing = false;
            SadConsole.Game.Instance.Window.AllowAltF4 = true;

            TryLoadGame();

            var mainMenu = new MainMenuView();
            mainMenu.Show();
        }

        private static void TryLoadGame()
        {
            GameManager.Current.LoadGame();
        }

        public static void Exit()
        {
            if (CurrentGame.Game != null)
            {
                new SaveManager().SaveGame();
            }

            SadConsole.Game.Instance.Exit();
        }
    }
}
