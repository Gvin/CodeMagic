using System.Windows.Forms;
using CodeMagic.Game.Spells;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Button = SadConsole.Controls.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CodeMagic.UI.Sad.Views
{
    public class LoadSpellFromLibraryView : SpellsLibraryViewBase
    {
        private Button closeButton;
        private Button okButton;

        public LoadSpellFromLibraryView()
        {
            InitializeControls();
        }

        public BookSpell SelectedSpell => SelectedItem?.Spell;

        private void InitializeControls()
        {
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };

            closeButton = new Button(15, 3)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Cancel",
                CanFocus = false,
                Theme = buttonsTheme
            };
            closeButton.Click += (sender, args) => Close(DialogResult.Cancel);
            Add(closeButton);

            okButton = new Button(15, 3)
            {
                Position = new Point(Width - 37, Height - 4),
                Text = "[ENTER] OK",
                CanFocus = false,
                Theme = buttonsTheme
            };
            okButton.Click += (sender, args) => Close(DialogResult.OK);
            Add(okButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Enter:
                    Close(DialogResult.OK);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }
    }
}