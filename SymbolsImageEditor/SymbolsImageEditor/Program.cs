using System;
using System.IO;
using System.Linq;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using SadConsole;

namespace SymbolsImageEditor
{
    static class Program
    {
        public const int Width = 100;
        public const int Height = 50;

        public static Font Font => LoadFont();

        [STAThread]
        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            Game.Create(Width * 2, Height);

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

            Global.CurrentScreen = new EditorWindow();
        }
    }
}