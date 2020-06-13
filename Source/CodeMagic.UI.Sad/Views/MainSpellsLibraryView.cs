using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Sad.Views
{
    public class MainSpellsLibraryView : SpellsLibraryViewBase
    {
        private StandardButton closeButton;

        public MainSpellsLibraryView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Close();
            Add(closeButton);
        }
    }
}