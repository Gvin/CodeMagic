using System;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.Views;
using SadConsole;

namespace CodeMagic.UI.Sad
{
    static class Program
    {
        public const int Width = 120;
        public const int Height = 50;

        public const int MapCellImageSize = 3;

        [STAThread]
        static void Main(string[] args)
        {
            GameConfigurator.Configure();
            FontProvider.InitializeFont();

            // Setup the engine and create the main window.
            var gameWidth = (int) Math.Floor(Width * FontProvider.FontSizeMultiplier);
            var gameHeight = (int) Math.Floor(Height * FontProvider.FontSizeMultiplier);
            Game.Create(gameWidth, gameHeight);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            Game.Instance.Window.Title = "C0de Mag1c";
            Game.Instance.Window.AllowUserResizing = false;

            var mainMenu = new MainMenuView();
            mainMenu.Show();
        }
    }
}
