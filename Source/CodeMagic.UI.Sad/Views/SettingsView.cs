using System;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Button = SadConsole.Controls.Button;

namespace CodeMagic.UI.Sad.Views
{
    public class SettingsView : GameViewBase, ISettingsView
    {
        private StandardButton browseForLauncherButton;
        private StandardButton closeButton;

        private Button prevFontSizeButton;
        private Button nextFontSizeButton;

        public SettingsView()
        {
            InitializeControls();
        }

        public string FontSizeName { get; set; }

        public string SpellEditorPath { get; set; }

        private void InitializeControls()
        {
            browseForLauncherButton = new StandardButton(20)
            {
                Position = new Point(2, 6),
                Text = "[B] Browse"
            };
            browseForLauncherButton.Click += (sender, args) => BrowseEditor?.Invoke(this, EventArgs.Empty);
            Add(browseForLauncherButton);

            closeButton = new StandardButton(20)
            {
                Position = new Point(2, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(closeButton);

            prevFontSizeButton = new Button(1)
            {
                Position = new Point(13, 10),
                Text = "<",
                CanFocus = false,
            };
            prevFontSizeButton.Click += (sender, args) => DecreaseFontSize?.Invoke(this, EventArgs.Empty);
            Add(prevFontSizeButton);

            nextFontSizeButton = new Button(1)
            {
                Position = new Point(21, 10),
                Text = ">",
                CanFocus = false
            };
            nextFontSizeButton.Click += (sender, args) => IncreaseFontSize?.Invoke(this, EventArgs.Empty);
            Add(nextFontSizeButton);
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 1, "Game Settings");

            surface.Print(2, 4, "Spell Editor Application:");
            surface.Print(2, 5, new ColoredString(SpellEditorPath, new Cell(Color.Gray, DefaultBackground)));

            surface.Print(2, 10, "Font Size:");
            surface.Clear(15, 10, 10);
            surface.Print(15, 10, new ColoredString(FontSizeName, Color.Gray, DefaultBackground));
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.B:
                    BrowseEditor?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler BrowseEditor;
        public event EventHandler IncreaseFontSize;
        public event EventHandler DecreaseFontSize;
        public event EventHandler Exit;
    }
}