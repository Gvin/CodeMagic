using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.Saving;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad
{
    internal static class Program
    {
        public const int MapCellImageSize = 7;

        [STAThread]
        internal static void Main()
        {
            try
            {
                GameConfigurator.Configure();
                FontProvider.InitializeFont();

                // Setup the engine and create the main window.
                var gameWidth = FontProvider.GetScreenWidth(FontTarget.Game);
                var gameHeight = FontProvider.GetScreenHeight(FontTarget.Game) / 2;
                SadConsole.Game.Create(gameWidth, gameHeight);

                // Hook the start event so we can add consoles to the system.
                SadConsole.Game.OnInitialize = Init;
                // Start the game.
                SadConsole.Game.Instance.Run();
                SadConsole.Game.Instance.Dispose();
            }
            catch (Exception e)
            {
                LogManager.GetLog(nameof(Program)).Fatal(e);
                throw;
            }
            
        }

        private static void Init()
        {
            SadConsole.Game.Instance.Window.Title = "C0de Mag1c";
            SadConsole.Game.Instance.Window.AllowUserResizing = false;
            SadConsole.Game.Instance.Window.AllowAltF4 = false;
            SadConsole.Game.Instance.Exiting += Instance_Exiting;

            TryLoadGame();

            var mainMenu = new MainMenuView();
            mainMenu.Show();
        }

        private static void TryLoadGame()
        {
            GameManager.Current.LoadGame();
        }

        private static void Instance_Exiting(object sender, EventArgs e)
        {
            if (CurrentGame.Game != null)
            {
                new SaveManager().SaveGame();
            }
        }
    }
}
