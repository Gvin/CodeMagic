using System;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class LoadSpellView : SpellsLibraryViewBase, ILoadSpellView
    {
        public event EventHandler Ok;

        public LoadSpellView()
        {
            var closeButton = new FramedButton(new Rectangle(Width - 17, Height - 4, 15, 3))
            {
                Text = "[ESC] Cancel"
            };
            closeButton.Click += (sender, args) => OnExit();
            Controls.Add(closeButton);

            var okButton = new FramedButton(new Rectangle(Width - 37, Height - 4, 15, 3))
            {
                Text = "[ENTER] OK"
            };
            okButton.Click += (sender, args) => Ok?.Invoke(this, EventArgs.Empty);
            Controls.Add(okButton);
        }

        public override bool ProcessKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Enter:
                    Ok?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }
    }
}