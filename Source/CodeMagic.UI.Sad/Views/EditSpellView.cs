using System;
using System.Windows.Forms;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Themes;
using Button = SadConsole.Controls.Button;
using TextBox = SadConsole.Controls.TextBox;

namespace CodeMagic.UI.Sad.Views
{
    public class EditSpellView : View
    {
        private Button okButton;
        private Button cancelButton;

        private TextBox spellNameTextBox;
        private TextBox manaCostTextBox;

        private Button launchEditorButton;

        private readonly string spellFilePath;

        public EditSpellView(string spellFilePath)
        {
            this.spellFilePath = spellFilePath;

            InitializeControls();
        }

        private void InitializeControls()
        {
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };

            okButton = new Button(20, 3)
            {
                Position = new Point(3, Height - 4),
                CanFocus = false,
                Theme = buttonsTheme,
                Text = "OK"
            };
            okButton.Click += okButton_Click;
            Add(okButton);

            cancelButton = new Button(20, 3)
            {
                Position = new Point(27, Height - 4),
                CanFocus = false,
                Theme = buttonsTheme,
                Text = "Cancel"
            };
            cancelButton.Click += cancelButton_Click;
            Add(cancelButton);

            launchEditorButton = new Button(40, 3)
            {
                Position = new Point(3, 12),
                CanFocus = false,
                Theme = buttonsTheme,
                Text = "Launch Code Editor"
            };
            launchEditorButton.Click += launchEditorButton_Click;
            Add(launchEditorButton);

            var textBoxTheme = new TextBoxTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255)),
                    Appearance_ControlFocused = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255))
                }
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
            Close(DialogResult.OK);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close(DialogResult.Cancel);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(3, 2, "Edit spell file, fill all data and click OK to continue.");
            Print(3, 4, "Spell Name:");
            Print(3, 7, "Mana Cost:");

            Print(3, Height - 5,
                new ColoredString("Don't forget to save spell file changes before pressing OK.",
                    new Cell(Color.Red, DefaultBackground)));
        }

        public string Name { get; set; }

        public int? InitialManaCost { get; set; }

        public int ManaCost { get; private set; }
    }
}