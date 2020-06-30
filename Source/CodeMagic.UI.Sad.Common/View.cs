using System.Collections.Generic;
using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Common
{
    public abstract class View : Window
    {
        protected readonly ILog Log;

        protected static readonly Color FrameColor = Color.Gray;

        private readonly List<IVisualControl> visualControls;

        protected View(ILog log, int width, int height, Font font)
            : base(width, height)
        {
            Font = font;
            Log = log;

            visualControls = new List<IVisualControl>();

            DefaultForeground = Color.White;
            DefaultBackground = Color.Black;

            var surface = new DrawingSurface(Width, Height)
            {
                Position = new Point(0, 0),
                OnDraw = (s, time) => DrawView(s.Surface),
                CanFocus = false,
                UseMouse = false,
                UseKeyboard = false
            };
            ControlHostComponent.Add(surface);
        }

        protected void AddVisualControl(IVisualControl control)
        {
            visualControls.Add(control);
        }

        protected void RemoveVisualControl(IVisualControl control)
        {
            visualControls.Remove(control);
        }

        protected override void OnShown()
        {
            Log.Debug($"Opened view of type {GetType().Name}");
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

        private void DrawFrame(ICellSurface surface)
        {
            surface.Fill(DefaultForeground, DefaultBackground, null);

            surface.Fill(1, 0, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            surface.Fill(1, Height - 1, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            surface.Print(0, 0, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╔')));
            surface.Print(Width - 1, 0, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╗')));

            surface.Print(0, Height - 1, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╚')));
            surface.Print(Width - 1, Height - 1, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('╝')));

            for (var y = 1; y < Height - 1; y++)
            {
                surface.Print(0, y, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('║')));
                surface.Print(Width - 1, y, new ColoredGlyph(FrameColor, DefaultBackground, Glyphs.GetGlyph('║')));
            }
        }

        protected virtual void DrawView(ICellSurface surface)
        {
            DrawFrame(surface);

            foreach (var control in visualControls)
            {
                if (!control.IsVisible)
                    continue;

                var controlSurface = surface.GetSubSurface(control.Position);
                control.Draw(controlSurface);
            }
        }
    }
}