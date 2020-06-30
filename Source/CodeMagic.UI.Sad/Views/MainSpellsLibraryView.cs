using CodeMagic.UI.Sad.Controls;
using SadRogue.Primitives;

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
            closeButton.Click += (sender, args) => Hide();
            ControlHostComponent.Add(closeButton);
        }
    }
}