using System;
using System.Linq;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Button = SadConsole.Controls.Button;

namespace CodeMagic.UI.Sad.Views
{
    public class SettingsView : GameViewBase
    {
        private StandardButton browseForLauncherButton;
        private StandardButton closeButton;

        private Button prevFontSizeButton;
        private Button nextFontSizeButton;

        public SettingsView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            browseForLauncherButton = new StandardButton(20)
            {
                Position = new Point(2, 6),
                Text = "[B] Browse"
            };
            browseForLauncherButton.Click += (sender, args) => BrowseForCodeEditor();
            Add(browseForLauncherButton);

            closeButton = new StandardButton(20)
            {
                Position = new Point(2, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Close();
            Add(closeButton);

            prevFontSizeButton = new Button(1)
            {
                Position = new Point(13, 10),
                Text = "<",
                CanFocus = false,
            };
            prevFontSizeButton.Click += (sender, args) => SwitchFontSize(false);
            Add(prevFontSizeButton);

            nextFontSizeButton = new Button(1)
            {
                Position = new Point(21, 10),
                Text = ">",
                CanFocus = false
            };
            nextFontSizeButton.Click += (sender, args) => SwitchFontSize(true);
            Add(nextFontSizeButton);
        }

        private void SwitchFontSize(bool forward)
        {
            var diff = forward ? 1 : -1;
            var size = Settings.Current.FontSize;
            var sizes = Enum.GetValues(typeof(FontSizeMultiplier)).Cast<FontSizeMultiplier>().ToList();

            var currentIndex = sizes.IndexOf(size);
            var nextIndex = currentIndex + diff;
            nextIndex = Math.Max(0, nextIndex);
            nextIndex = Math.Min(sizes.Count - 1, nextIndex);

            Settings.Current.FontSize = sizes[nextIndex];
            Settings.Current.Save();
        }

        private string GetFontSizeName(FontSizeMultiplier fontSize)
        {
            switch (fontSize)
            {
                case FontSizeMultiplier.X1:
                    return "x1";
                case FontSizeMultiplier.X2:
                    return "x2";
                default:
                    throw new ArgumentException($"Unknown font size: {fontSize}");
            }
        }

        private string GetCurrentFontSizeName()
        {
            return GetFontSizeName(Settings.Current.FontSize);
        }

        private void BrowseForCodeEditor()
        {
            // TODO: Replace browse dialog
            throw new NotImplementedException();
            // var browseDialog = new OpenFileDialog
            // {
            //     Title = "Browse For Code Editor",
            //     CheckFileExists = true,
            //     Filter = "Application|*.exe",
            //     Multiselect = false,
            //     FileName = Properties.Settings.Default.SpellEditorPath
            // };
            //
            // if (browseDialog.ShowDialog() == DialogResult.OK)
            // {
            //     Properties.Settings.Default.SpellEditorPath = browseDialog.FileName;
            //     Properties.Settings.Default.Save();
            // }
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 1, "Game Settings");

            surface.Print(2, 4, "Spell Editor Application:");
            surface.Print(2, 5, new ColoredString(Settings.Current.SpellEditorPath, new Cell(Color.Gray, DefaultBackground)));

            surface.Print(2, 10, "Font Size:");
            surface.Clear(15, 10, 10);
            surface.Print(15, 10, new ColoredString(GetCurrentFontSizeName(), Color.Gray, DefaultBackground));
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.B:
                    BrowseForCodeEditor();
                    return true;
                case Keys.Escape:
                    Close();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }
    }
}