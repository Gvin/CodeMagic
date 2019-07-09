using System.Text;

namespace CodeMagic.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleFontHelper.SetConsoleFont();
            Colorful.Console.Title = "Code Spell";
            Colorful.Console.OutputEncoding = Encoding.Unicode;
            Colorful.Console.CursorVisible = false;
            Colorful.Console.WindowHeight = 50;

            ViewsManager.Current.Start();
        }
    }
}
