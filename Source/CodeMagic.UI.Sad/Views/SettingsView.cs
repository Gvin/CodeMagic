using System;
using System.Linq;
using System.Windows.Forms;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Button = SadConsole.Controls.Button;

namespace CodeMagic.UI.Sad.Views
{
    public class SettingsView : View
    {
        private StandardButton browseForLauncherButton;
        private StandardButton closeButton;

        private Button prevFontSizeButton;
        private Button nextFontSizeButton;

        public SettingsView() : base(Program.Width, Program.Height)
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
                Position = new Point(23, 10),
                Text = ">",
                CanFocus = false
            };
            nextFontSizeButton.Click += (sender, args) => SwitchFontSize(true);
            Add(nextFontSizeButton);
        }

        private void SwitchFontSize(bool forward)
        {
            var diff = forward ? 1 : -1;
            var size = (Font.FontSizes) Enum.Parse(typeof(Font.FontSizes), Properties.Settings.Default.FontSize);
            var sizes = Enum.GetValues(typeof(Font.FontSizes)).Cast<Font.FontSizes>().ToList();

            var currentIndex = sizes.IndexOf(size);
            var nextIndex = currentIndex + diff;
            nextIndex = Math.Max(0, nextIndex);
            nextIndex = Math.Min(sizes.Count - 1, nextIndex);

            Properties.Settings.Default.FontSize = sizes[nextIndex].ToString();
            Properties.Settings.Default.Save();
        }

        private void BrowseForCodeEditor()
        {
            var browseDialog = new OpenFileDialog
            {
                Title = "Browse For Code Editor",
                CheckFileExists = true,
                Filter = "Application|*.exe",
                Multiselect = false,
                FileName = Properties.Settings.Default.SpellEditorPath
            };

            if (browseDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.SpellEditorPath = browseDialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, "Game Settings");

            Print(2, 4, "Spell Editor Application:");
            Print(2, 5, new ColoredString(Properties.Settings.Default.SpellEditorPath, new Cell(Color.Gray, DefaultBackground)));

            Print(2, 10, "Font Size:");
            Clear(15, 10, 10);
            Print(15, 10, new ColoredString(Properties.Settings.Default.FontSize, Color.Gray, DefaultBackground));
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