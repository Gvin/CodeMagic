using System;
using System.Linq;
using System.Windows.Forms;
using CodeMagic.Core.Logging;
using CodeMagic.Game;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;

namespace CodeMagic.UI.Sad.Views
{
    public abstract class View : ControlsConsole
    {
        private static readonly ILog Log = LogManager.GetLog<View>();

        protected static readonly Color FrameColor = Color.Gray;

        public event EventHandler<ViewClosedEventArgs> Closed;

        protected View()
            : this(FontTarget.Interface)
        {
        }

        protected View(FontTarget font)
            : base(FontProvider.GetScreenWidth(font), FontProvider.GetScreenHeight(font), FontProvider.GetFont(font))
        {
            DefaultForeground = Color.White;
            DefaultBackground = Color.Black;
            DrawFrame();
        }

        public void Show()
        {
            Log.Debug($"Opening view of type {GetType().Name}");

            ViewsManager.Current.AddView(this);
            Global.CurrentScreen = this;
            OnShown();

            Log.Debug($"Opened view of type {GetType().Name}");
        }

        protected virtual void OnShown()
        {
            // Do nothing
        }

        public void Close(DialogResult? result = null)
        {
            Log.Debug($"Closing view of type {GetType().Name}");

            ViewsManager.Current.RemoveView(this);
            Global.CurrentScreen = ViewsManager.Current.CurrentView;

            OnClosed(result);
            Closed?.Invoke(this, new ViewClosedEventArgs(result));

            Log.Debug($"Closed view of type {GetType().Name}");
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

        protected void DrawImage( int x, int y, SymbolsImage image, Color defaultForeColor, Color defaultBackColor)
        {
            for (int posY = 0; posY < image.Height; posY++)
            {
                for (int posX = 0; posX < image.Width; posX++)
                {
                    var pixel = image[posX, posY];
                    var backColor = pixel.BackgroundColor?.ToXna() ?? defaultBackColor;

                    var printX = x + posX;
                    var printY = y + posY;

                    if (pixel.Symbol.HasValue)
                    {
                        var foreColor = pixel.Color?.ToXna() ?? defaultForeColor;
                        Print(printX, printY,
                            new ColoredGlyph(pixel.Symbol.Value, foreColor, backColor));
                    }
                    else
                    {
                        Print(printX, printY, new ColoredGlyph(' ', defaultForeColor, backColor));
                    }
                }
            }
        }

        public void PrintStyledText(int x, int y, params ColoredString[] text)
        {
            var glyphs = text.SelectMany(part => part.ToArray()).ToArray();
            var coloredString = new ColoredString(glyphs);
            Print(x, y, coloredString);
        }

        public void PrintStyledText(int x, int y, StyledLine text)
        {
            PrintStyledText(x, y, text.Select(part =>
                new ColoredString(ConvertString(part.String), part.TextColor.ToXna(), DefaultBackground)).ToArray());
        }

        private string ConvertString(string initial)
        {
            var result = string.Empty;
            foreach (var symbol in initial)
            {
                result += (char)Glyphs.GetGlyph(symbol);
            }

            return result;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.KeysPressed.Count == 1)
            {
                var keyPressed = info.KeysPressed[0];
                return ProcessKeyPressed(keyPressed) || base.ProcessKeyboard(info);
            }

            if (info.KeysDown.Count == 1)
            {
                var keyDown = info.KeysDown[0];
                return ProcessKeyDown(keyDown) || base.ProcessKeyboard(info);
            }

            return base.ProcessKeyboard(info);
        }

        protected virtual bool ProcessKeyPressed(AsciiKey key)
        {
            return false;
        }

        protected virtual bool ProcessKeyDown(AsciiKey key)
        {
            return false;
        }

        private void DrawFrame()
        {
            Fill(DefaultForeground, DefaultBackground, null);

            Fill(1, 0, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            Fill(1, Height - 1, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));

            Print(0, 0, new ColoredGlyph(Glyphs.GetGlyph('╔'), FrameColor, DefaultBackground));
            Print(Width - 1, 0, new ColoredGlyph(Glyphs.GetGlyph('╗'), FrameColor, DefaultBackground));

            Print(0, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╚'), FrameColor, DefaultBackground));
            Print(Width - 1, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╝'), FrameColor, DefaultBackground));

            for (var y = 1; y < Height - 1; y++)
            {
                Print(0, y, new ColoredGlyph(Glyphs.GetGlyph('║'), FrameColor, DefaultBackground));
                Print(Width - 1, y, new ColoredGlyph(Glyphs.GetGlyph('║'), FrameColor, DefaultBackground));
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