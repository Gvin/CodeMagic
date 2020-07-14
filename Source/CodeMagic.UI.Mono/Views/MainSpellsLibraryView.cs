using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Views
{
    public class MainSpellsLibraryView : SpellsLibraryViewBase, IMainSpellsLibraryView
    {
        public MainSpellsLibraryView()
        {
            var closeButton = new FramedButton(new Rectangle(Width - 17, Height - 4, 15, 3))
            {
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => OnExit();
            Controls.Add(closeButton);
        }
    }
}