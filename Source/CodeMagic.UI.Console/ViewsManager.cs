using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.UI.Console.Drawing.Writing;
using CodeMagic.UI.Console.Views;

namespace CodeMagic.UI.Console
{
    public class ViewsManager
    {
        private static readonly Color FrameColor = Color.Gray;

        public static ViewsManager Current { get; } = new ViewsManager();

        private readonly List<IView> viewsStack;
        private bool isWorking;

        private ViewsManager()
        {
            viewsStack = new List<IView>();
        }

        public void AddView(IView view)
        {
            viewsStack.Add(view);
            DrawUI();
            DrawContent();
        }

        public void RemoveView(IView view)
        {
            viewsStack.Remove(view);
            DrawUI();
            DrawContent();
        }

        public void Start()
        {
            viewsStack.Clear();
            viewsStack.Add(new MenuView());

            isWorking = true;

            DrawUI();
            while (isWorking)
            {
                DrawContent();
                var keyInfo = Colorful.Console.ReadKey(true);
                ProcessKey(keyInfo);
            }
        }

        public void Exit()
        {
            isWorking = false;
        }

        private void DrawUI()
        {
            Writer.BackColor = Color.Black;
            Writer.CursorVisible = false;
            Writer.Clear();

            DrawFrame();
            var lastView = viewsStack.LastOrDefault();
            lastView?.DrawStatic();
        }

        private void DrawContent()
        {
            var lastView = viewsStack.LastOrDefault();
            lastView?.DrawDynamic();
        }

        private void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.F5:
                    DrawUI();
                    DrawContent();
                    break;
                default:
                    var lastView = viewsStack.LastOrDefault();
                    lastView?.ProcessKey(keyInfo);
                    break;
            }
        }

        private void DrawFrame()
        {
            Writer.DrawFrame(0, 0, Writer.ScreenWidth, Writer.ScreenHeight - 1, true, FrameColor, Color.Black);

            Writer.WriteAt(2, 0, "[F5-Refresh UI]", Color.White);
            Writer.WriteAt(Writer.ScreenWidth - 12, 0, "[ESC-Exit]", Color.White);
        }
    }
}