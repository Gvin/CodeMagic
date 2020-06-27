using System;
using System.Collections.Generic;
using CodeMagic.UI.Sad.Views;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Common
{
    public abstract class View : ControlsConsole
    {
        protected readonly ILog Log;

        protected static readonly Color FrameColor = Color.Gray;

        public event EventHandler<ViewClosedEventArgs> Closed;

        private readonly List<IVisualControl> visualControls;

        protected View(ILog log, int width, int height, Font font)
            : base(width, height, font)
        {
            Log = log;

            visualControls = new List<IVisualControl>();

            DefaultForeground = Color.White;
            DefaultBackground = Color.Black;

            ThemeColors = new Colors
            {
                Appearance_ControlNormal = new Cell(Color.White, Color.Black),
                Appearance_ControlDisabled = new Cell(Color.White, Color.Black)
            };

            var surface = new DrawingSurface(Width, Height)
            {
                Position = new Point(0, 0),
                OnDraw = s => DrawView(s.Surface),
                CanFocus = false,
                UseMouse = false,
                UseKeyboard = false
            };
            Add(surface);
        }

        protected void AddVisualControl(IVisualControl control)
        {
            visualControls.Add(control);
        }

        protected void RemoveVisualControl(IVisualControl control)
        {
            visualControls.Remove(control);
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

        public void Close(DialogResult result = DialogResult.None)
        {
            Log.Debug($"Closing view of type {GetType().Name}");

            ViewsManager.Current.RemoveView(this);
            Global.CurrentScreen = ViewsManager.Current.CurrentView;

            OnClosed(result);
            Closed?.Invoke(this, new ViewClosedEventArgs(result));

            Log.Debug($"Closed view of type {GetType().Name}");
        }

        protected virtual void OnClosed(DialogResult result)
        {
            // Do nothing
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

        private void DrawFrame(CellSurface surface)
        {
            surface.Fill(DefaultForeground, DefaultBackground, null);

            surface.Fill(1, 0, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            surface.Fill(1, Height - 1, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('═'));
            surface.Print(0, 0, new ColoredGlyph(Glyphs.GetGlyph('╔'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, 0, new ColoredGlyph(Glyphs.GetGlyph('╗'), FrameColor, DefaultBackground));

            surface.Print(0, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╚'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╝'), FrameColor, DefaultBackground));

            for (var y = 1; y < Height - 1; y++)
            {
                surface.Print(0, y, new ColoredGlyph(Glyphs.GetGlyph('║'), FrameColor, DefaultBackground));
                surface.Print(Width - 1, y, new ColoredGlyph(Glyphs.GetGlyph('║'), FrameColor, DefaultBackground));
            }
        }

        protected virtual void DrawView(CellSurface surface)
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

    public class ViewClosedEventArgs : EventArgs
    {
        public ViewClosedEventArgs(DialogResult result = DialogResult.Ok)
        {
            Result = result;
        }

        public DialogResult Result { get; }
    }

    public enum DialogResult
    {
        None,
        Cancel,
        Ok
    }
}