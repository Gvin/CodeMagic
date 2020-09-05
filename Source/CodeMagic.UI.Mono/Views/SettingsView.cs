using System;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class SettingsView : BaseWindow, ISettingsView
    {
        public SettingsView() : base(FontTarget.Interface)
        {
            var closeButton = new FramedButton(new Rectangle(2, Height - 4, 20, 3))
            {
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(closeButton);
        }

        public override bool ProcessKeysPressed(Keys[] keys)
        {
            if (keys.Length == 1)
            {
                switch (keys[0])
                {
                    case Keys.Escape:
                        Exit?.Invoke(this, EventArgs.Empty);
                        return true;
                }
            }

            return base.ProcessKeysPressed(keys);
        }

        public event EventHandler BrowseEditor;
        public event EventHandler IncreaseFontSize;
        public event EventHandler DecreaseFontSize;
        public event EventHandler Exit;

        public string FontSizeName { get; set; }
        public string SpellEditorPath { get; set; }
    }
}