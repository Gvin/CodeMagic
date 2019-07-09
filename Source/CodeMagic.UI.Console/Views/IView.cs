using System;

namespace CodeMagic.UI.Console.Views
{
    public interface IView
    {
        void DrawStatic();

        void DrawDynamic();

        void ProcessKey(ConsoleKeyInfo keyInfo);

        void Close();

        void Show();
    }
}