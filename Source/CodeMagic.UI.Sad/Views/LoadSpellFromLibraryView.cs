using CodeMagic.Game.Spells;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CodeMagic.UI.Sad.Views
{
    public class LoadSpellFromLibraryView : SpellsLibraryViewBase
    {
        private StandardButton closeButton;
        private StandardButton okButton;

        public LoadSpellFromLibraryView()
        {
            InitializeControls();
        }

        public BookSpell SelectedSpell => SelectedItem?.Spell;

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Cancel"
            };
            closeButton.Click += (sender, args) => Close(DialogResult.Cancel);
            Add(closeButton);

            okButton = new StandardButton(15)
            {
                Position = new Point(Width - 37, Height - 4),
                Text = "[ENTER] OK"
            };
            okButton.Click += (sender, args) => Close(DialogResult.Ok);
            Add(okButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Enter:
                    Close(DialogResult.Ok);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }
    }
}