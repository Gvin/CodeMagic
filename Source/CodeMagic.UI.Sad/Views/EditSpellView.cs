using System;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using SadConsole;
using SadConsole.Readers;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using SadRogue.Primitives;
using Point = SadRogue.Primitives.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class EditSpellView : GameViewBase
    {
        private StandardButton okButton;
        private StandardButton cancelButton;

        private TextBox spellNameTextBox;
        private TextBox manaCostTextBox;

        private StandardButton launchEditorButton;

        private readonly string spellFilePath;

        public EditSpellView(string spellFilePath)
        {
            this.spellFilePath = spellFilePath;

            InitializeControls();
        }

        private void InitializeControls()
        {
            okButton = new StandardButton(20)
            {
                Position = new Point(3, Height - 4),
                Text = "OK"
            };
            okButton.Click += okButton_Click;
            ControlHostComponent.Add(okButton);

            cancelButton = new StandardButton(20)
            {
                Position = new Point(27, Height - 4),
                Text = "Cancel"
            };
            cancelButton.Click += cancelButton_Click;
            ControlHostComponent.Add(cancelButton);

            launchEditorButton = new StandardButton(40)
            {
                Position = new Point(3, 12),
                Text = "Launch Code Editor"
            };
            launchEditorButton.Click += launchEditorButton_Click;
            ControlHostComponent.Add(launchEditorButton);

            var textBoxTheme = new TextBoxTheme
            {
                Normal = new ColoredGlyph(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255)),
                Focused = new ColoredGlyph(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255))
            };
            spellNameTextBox = new TextBox(60)
            {
                Position = new Point(3, 5),
                Theme = textBoxTheme,
                MaxLength = 50
            };
            ControlHostComponent.Add(spellNameTextBox);

            manaCostTextBox = new TextBox(15)
            {
                Position = new Point(3, 8),
                IsNumeric = true,
                AllowDecimal = false,
                Theme = textBoxTheme,
                MaxLength = 10
            };
            ControlHostComponent.Add(manaCostTextBox);
        }

        private void launchEditorButton_Click(object sender, EventArgs e)
        {
            EditSpellHelper.LaunchSpellFileEditor(spellFilePath);
        }

        protected override void OnShown()
        {
            base.OnShown();

            spellNameTextBox.Text = Name ?? string.Empty;
            manaCostTextBox.Text = InitialManaCost.HasValue ? InitialManaCost.Value.ToString() : 0.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Name = spellNameTextBox.EditingText;
            ManaCost = int.Parse(manaCostTextBox.EditingText ?? manaCostTextBox.Text);
            DialogResult = true;
            Hide();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Hide();
        }


        protected override void DrawView(ICellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(3, 2, "Edit spell file, fill all data and click OK to continue.");
            surface.Print(3, 4, "Spell Name:");
            surface.Print(3, 7, "Mana Cost:");

            surface.Print(3, Height - 5,
                new ColoredString("Don't forget to save spell file changes before pressing OK.",
                    Color.Red, DefaultBackground));
        }

        public string Name { get; set; }

        public int? InitialManaCost { get; set; }

        public int ManaCost { get; private set; }
    }
}