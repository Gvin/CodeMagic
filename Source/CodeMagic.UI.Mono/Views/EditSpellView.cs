using System;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Views
{
    public class EditSpellView : BaseWindow, IEditSpellView
    {
        private readonly Input spellNameInput;
        private readonly Input manaCostInput;

        public EditSpellView()
            : base(FontTarget.Interface)
        {
            var descriptionLabel = new Label(3, 2)
            {
                Text = "Edit spell file, fill all data and click OK to continue."
            };
            Controls.Add(descriptionLabel);

            var okButton = new FramedButton(new Rectangle(3, Height - 4, 20, 3))
            {
                Text = "OK"
            };
            okButton.Click += (sender, args) => Ok?.Invoke(this, EventArgs.Empty);
            Controls.Add(okButton);

            var cancelButton = new FramedButton(new Rectangle(27, Height - 4, 20, 3))
            {
                Text = "Cancel"
            };
            cancelButton.Click += (sender, args) => Cancel?.Invoke(this, EventArgs.Empty);
            Controls.Add(cancelButton);

            var warningLabel = new Label(3, Height - 5)
            {
                Text = "Don't forget to save spell file changes before pressing OK.",
                ForeColor = Color.Red
            };
            Controls.Add(warningLabel);

            var launchEditorButton = new FramedButton(new Rectangle(3, 12, 40, 3))
            {
                Text = "Launch Code Editor"
            };
            launchEditorButton.Click += (sender, args) => LaunchEditor?.Invoke(this, EventArgs.Empty);
            Controls.Add(launchEditorButton);

            var spellNameLabel = new Label(3, 4)
            {
                Text = "Spell Name:"
            };
            Controls.Add(spellNameLabel);

            spellNameInput = new Input(new Rectangle(3, 5, 60, 1));
            Controls.Add(spellNameInput);

            var manaCostLabel = new Label(3, 7)
            {
                Text = "Mana Cost:"
            };
            Controls.Add(manaCostLabel);

            manaCostInput = new Input(new Rectangle(3, 8, 15, 1))
            {
                FilterRegex = @"^\d*$"
            };
            Controls.Add(manaCostInput);
        }

        public event EventHandler Ok;
        public event EventHandler Cancel;
        public event EventHandler LaunchEditor;

        public string SpellName
        {
            get => spellNameInput.Text;
            set => spellNameInput.Text = value;

        }

        public int ManaCost
        {
            get => int.Parse(manaCostInput.Text ?? "0");
            set => manaCostInput.Text = value.ToString();
        }
    }
}