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

        static void Main(string[] args)
        {
            GameConfigurator.Configure();

            // Setup the engine and create the main window.
            Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            var mainMenu = new MainMenuView();
            mainMenu.Show();
        }
    }
}
