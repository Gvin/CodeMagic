using System;
using CodeMagic.Core.Game;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.Saving;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad
{
    static class Program
    {
        public const int Width = 100;
        public const int Height = 50;

        public const int MapCellImageSize = 3;

        [STAThread]
        static void Main()
        {
            GameConfigurator.Configure();
            FontProvider.InitializeFont(new WinFormsScreenSizeProvider(), Width, Height);

            // Setup the engine and create the main window.
            var gameWidth = (int) Math.Floor(Width * FontProvider.FontHorizontalMultiplier);
            var gameHeight = (int) Math.Floor(Height * FontProvider.FontVerticalMultiplier);
            SadConsole.Game.Create(gameWidth, gameHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;
            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
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
