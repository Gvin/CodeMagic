using System.Text;
using CodeMagic.UI.Console.Drawing.Writing;
using CodeMagic.UI.Console.Drawing.Writing.WriterImplementation;

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

            Writer.Initialize(new ColorfulConsoleWriter());
            Writer.ScreenHeight = 50;

            ViewsManager.Current.Start();
        }
    }
}
