﻿using System;
using System.Windows.Forms;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CodeMagic.UI.Sad.Views
{
    public class View : ControlsConsole
    {
        public static readonly Color FrameColor = Color.Gray;

        public event EventHandler<ViewClosedEventArgs> Closed;

        public View(int width, int height) 
            : base(width, height)
        {
            DefaultForeground = Color.White;
            DefaultBackground = Color.Black;
            DrawFrame();
        }

        public void Show()
        {
            ViewsManager.Current.AddView(this);
            Global.CurrentScreen = this;
            OnShown();
        }

        protected virtual void OnShown()
        {
            // Do nothing
        }

        public void Close(DialogResult? result = null)
        {
            ViewsManager.Current.RemoveLastView();
            Global.CurrentScreen = ViewsManager.Current.CurrentView;

            OnClosed(result);
            Closed?.Invoke(this, new ViewClosedEventArgs(result));
        }

        protected virtual void OnClosed(DialogResult? result)
        {
            // Do nothing
        }

        protected void DrawVerticalLine(int x, int y, int length, ColoredGlyph coloredGlyph)
        {
            for (var dY = 0; dY < length; dY++)
            {
                Print(x, y + dY, coloredGlyph);
            }
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.KeysPressed.Count != 1)
                return base.ProcessKeyboard(info);

            var key = info.KeysPressed[0];
            if (ProcessKeyPressed(key))
                return true;

            return base.ProcessKeyboard(info);
        }

        protected virtual bool ProcessKeyPressed(AsciiKey key)
        {
            // Do nothing
            return false;
        }

        private void DrawFrame()
        {
            Fill(DefaultForeground, DefaultBackground, null);

            Fill(1, 0, Width - 2, FrameColor, DefaultBackground, Glyphs.GlyphBoxDoubleHorizontal);
            Fill(1, Height - 1, Width - 2, FrameColor, DefaultBackground, Glyphs.GlyphBoxDoubleHorizontal);

            Print(0, 0, new ColoredGlyph(Glyphs.GlyphBoxDoubleDownRight, FrameColor, DefaultBackground));
            Print(Width - 1, 0, new ColoredGlyph(Glyphs.GlyphBoxDoubleDownLeft, FrameColor, DefaultBackground));

            Print(0, Height - 1, new ColoredGlyph(Glyphs.GlyphBoxDoubleUpRight, FrameColor, DefaultBackground));
            Print(Width - 1, Height - 1, new ColoredGlyph(Glyphs.GlyphBoxDoubleUpLeft, FrameColor, DefaultBackground));

            for (var y = 1; y < Height - 1; y++)
            {
                Print(0, y, new ColoredGlyph(Glyphs.GlyphBoxDoubleVertical, FrameColor, DefaultBackground));
                Print(Width - 1, y, new ColoredGlyph(Glyphs.GlyphBoxDoubleVertical, FrameColor, DefaultBackground));
            }
        }
    }

    public class ViewClosedEventArgs : EventArgs
    {
        public ViewClosedEventArgs(DialogResult? result = null)
        {
            Result = result;
        }

        public DialogResult? Result { get; set; }
    }
}