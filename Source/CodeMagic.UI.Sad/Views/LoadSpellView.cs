using System;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CodeMagic.UI.Sad.Views
{
    public class LoadSpellView : SpellsLibraryViewBase, ILoadSpellView
    {
        private StandardButton closeButton;
        private StandardButton okButton;

        public LoadSpellView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Cancel"
            };
            closeButton.Click += (sender, args) => OnExit();
            Add(closeButton);

            okButton = new StandardButton(15)
            {
                Position = new Point(Width - 37, Height - 4),
                Text = "[ENTER] OK"
            };
            okButton.Click += (sender, args) => Ok?.Invoke(this, EventArgs.Empty);
            Add(okButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Enter:
                    Ok?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler Ok;
    }
}