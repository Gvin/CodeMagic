using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Views;
using Writer = Colorful.Console;

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
            Writer.BackgroundColor = Color.Black;
            Writer.CursorVisible = false;
            Writer.Clear();
            Writer.BufferHeight = Writer.WindowHeight;

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
            DrawTopHintBox();
            DrawSideLines();
            DrawBottomLine();
        }

        private void DrawTopHintBox()
        {
            Writer.CursorTop = 0;
            Writer.CursorLeft = 0;
            Writer.BackgroundColor = Color.Black;
            Writer.Write("" + LineTypes.DoubleDownAndRight + LineTypes.DoubleHorizontal, FrameColor);
            Writer.Write("[F5-Refresh UI]", Color.White);
            Writer.Write(LineTypes.DoubleHorizontal, FrameColor);
            Writer.Write("[ESC-Exit]", Color.White);
            DrawingHelper.DrawHorizontalLine(Writer.CursorTop, Writer.CursorLeft, Writer.WindowWidth - 2, true, FrameColor);
            Writer.Write(LineTypes.DoubleDownAndLeft, FrameColor);
        }

        private void DrawSideLines()
        {
            for (var top = 1; top < Writer.WindowHeight - 2; top++)
            {
                Writer.CursorTop = top;
                Writer.CursorLeft = 0;
                Writer.Write(LineTypes.DoubleVertical, FrameColor);
                Writer.CursorLeft = Writer.WindowWidth - 1;
                Writer.Write(LineTypes.DoubleVertical, FrameColor);
            }
        }

        private void DrawBottomLine()
        {
            Writer.Write(LineTypes.DoubleUpAndRight, FrameColor);
            DrawingHelper.DrawHorizontalLine(Writer.CursorTop, 1, Writer.WindowWidth - 2, true, FrameColor);
            Writer.Write(LineTypes.DoubleUpAndLeft, FrameColor);
        }
    }
}