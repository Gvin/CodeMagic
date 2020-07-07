using System;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Themes;
using TextBox = SadConsole.Controls.TextBox;

namespace CodeMagic.UI.Sad.Views
{
    public class EditSpellView : GameViewBase, IEditSpellView
    {
        private StandardButton okButton;
        private StandardButton cancelButton;

        private TextBox spellNameTextBox;
        private TextBox manaCostTextBox;

        private StandardButton launchEditorButton;

        public EditSpellView()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            okButton = new StandardButton(20)
            {
                Position = new Point(3, Height - 4),
                Text = "OK"
            };
            okButton.Click += (sender, args) => Ok?.Invoke(this, EventArgs.Empty);
            Add(okButton);

            cancelButton = new StandardButton(20)
            {
                Position = new Point(27, Height - 4),
                Text = "Cancel"
            };
            cancelButton.Click += (sender, args) => Cancel?.Invoke(this, EventArgs.Empty);
            Add(cancelButton);

            launchEditorButton = new StandardButton(40)
            {
                Position = new Point(3, 12),
                Text = "Launch Code Editor"
            };
            launchEditorButton.Click += (sender, args) => LaunchEditor?.Invoke(this, EventArgs.Empty);
            Add(launchEditorButton);

            var textBoxTheme = new TextBoxTheme
            {
                Normal = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255)),
                Focused = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255))
            };
            spellNameTextBox = new TextBox(60)
            {
                Position = new Point(3, 5),
                Theme = textBoxTheme,
                MaxLength = 50
            };
            Add(spellNameTextBox);

            manaCostTextBox = new TextBox(15)
            {
                Position = new Point(3, 8),
                IsNumeric = true,
                AllowDecimal = false,
                Theme = textBoxTheme,
                MaxLength = 10
            };
            Add(manaCostTextBox);
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(3, 2, "Edit spell file, fill all data and click OK to continue.");
            surface.Print(3, 4, "Spell Name:");
            surface.Print(3, 7, "Mana Cost:");

            surface.Print(3, Height - 5,
                new ColoredString("Don't forget to save spell file changes before pressing OK.",
                    new Cell(Color.Red, DefaultBackground)));
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler Ok;
        public event EventHandler Cancel;
        public event EventHandler LaunchEditor;

        public string SpellName
        {
            get => spellNameTextBox.EditingText;
            set => spellNameTextBox.Text = value;

        }

        public int ManaCost
        {
            get => int.Parse(manaCostTextBox.EditingText ?? manaCostTextBox.Text);
            set => manaCostTextBox.Text = value.ToString();
        }
    }
}