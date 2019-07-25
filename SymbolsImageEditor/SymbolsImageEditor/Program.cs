using System;
using SadConsole;

namespace SymbolsImageEditor
{
    static class Program
    {
        public const int Width = 120;
        public const int Height = 50;

        [STAThread]
        static void Main(string[] args)
        {
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
            Global.CurrentScreen = new EditorWindow();
        }
    }
}