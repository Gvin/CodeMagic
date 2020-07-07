using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Sad.Views
{
    public class MainSpellsLibraryView : SpellsLibraryViewBase, IMainSpellsLibraryView
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
            closeButton.Click += (sender, args) => OnExit();
            Add(closeButton);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }
    }
}