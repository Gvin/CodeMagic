using System;
using SadConsole;

namespace SymbolsImageEditor
{
    static class Program
    {
        public const int Width = 70;
        public const int Height = 30;

        public static Font Font => LoadFont();

        [STAThread]
        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create((int)Math.Floor(Width * 1.75f), Height);

            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static Font LoadFont()
        {
            return Global.LoadFont(@".\Resources\font_x075.font").GetFont(Font.FontSizes.One);
        }

        private static void Init()
        {
            var window = new EditorWindow();
            window.Show();
        }
    }
}