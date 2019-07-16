using System;

namespace CodeMagic.UI.Console.Views
{
    public interface IView
    {
        void ProcessKey(ConsoleKeyInfo keyInfo);

        void Close();

        void Show();

        void DrawStatic();

        void DrawDynamic();
    }
}