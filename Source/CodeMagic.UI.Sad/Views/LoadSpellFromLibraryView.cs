using CodeMagic.Game.Spells;
using CodeMagic.UI.Sad.Controls;
using SadConsole.Input;
using SadRogue.Primitives;

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
            closeButton.Click += (sender, args) =>
            {
                DialogResult = false;
                Hide();
            };
            ControlHostComponent.Add(closeButton);

            okButton = new StandardButton(15)
            {
                Position = new Point(Width - 37, Height - 4),
                Text = "[ENTER] OK"
            };
            okButton.Click += (sender, args) =>
            {
                DialogResult = true;
                Hide();
            };
            ControlHostComponent.Add(okButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Enter:
                    DialogResult = true;
                    Hide();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }
    }
}