using System;
using System.Windows.Forms;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CodeMagic.UI.Sad.Views
{
    public class SettingsView : View
    {
        private StandardButton browseForLauncherButton;
        private StandardButton closeButton;

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

            Print(2, 4, "Spell Editor Application");
            Print(2, 5, new ColoredString(Properties.Settings.Default.SpellEditorPath, new Cell(Color.Gray, DefaultBackground)));
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