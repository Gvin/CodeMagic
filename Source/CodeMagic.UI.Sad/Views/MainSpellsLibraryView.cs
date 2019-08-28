using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Views
{
    public class MainSpellsLibraryView : SpellsLibraryViewBase
    {
        private Button closeButton;

        public MainSpellsLibraryView()
        {
            InitializeControls();
        }

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
                Text = "[ESC] Close",
                CanFocus = false,
                Theme = buttonsTheme
            };
            closeButton.Click += (sender, args) => Close();
            Add(closeButton);
        }
    }
}