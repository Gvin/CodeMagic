using System;
using System.Linq;
using System.Windows.Forms;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.SpellsLibrary;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Button = SadConsole.Controls.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Orientation = SadConsole.Orientation;
using ScrollBar = SadConsole.Controls.ScrollBar;
using TextBox = SadConsole.Controls.TextBox;

namespace CodeMagic.UI.Sad.Views
{
    public abstract class SpellsLibraryViewBase : View
    {
        private const string DefaultSpellName = "New Spell";

        private CustomListBox<SpellListBoxItem> spellsList;
        private SpellDetailsControl spellDetails;
        private TextBox filterTextBox;

        private Button removeSpellButton;
        private Button editSpellButton;

        private string oldFilter;

        private readonly SpellsLibraryManager libraryManager;

        protected SpellsLibraryViewBase() 
            : base(Program.Width, Program.Height)
        {
            libraryManager = new SpellsLibraryManager();

            InitializeControls();
        }

        protected SpellListBoxItem SelectedItem => spellsList.SelectedItem;

        private void InitializeControls()
        {
            var buttonsTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };

            removeSpellButton = new Button(25, 3)
            {
                Position = new Point(Width - 57, 16),
                Text = "[R] Remove from Library",
                CanFocus = false,
                Theme = buttonsTheme
            };
            removeSpellButton.Click += (sender, args) => RemoveSpellFromLibrary();
            Add(removeSpellButton);

            editSpellButton = new Button(25, 3)
            {
                Position = new Point(Width - 57, 13),
                Text = "[E] Edit Spell",
                CanFocus = false,
                Theme = buttonsTheme
            };
            editSpellButton.Click += (sender, args) => EditSelectedSpell();
            Add(editSpellButton);

            spellDetails = new SpellDetailsControl(57, Height - 10)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(spellDetails);

            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };
            var scrollBar = new ScrollBar(Orientation.Vertical, Height - 6)
            {
                Position = new Point(Width - 60, 5),
                Theme = scrollBarTheme
            };
            Add(scrollBar);
            spellsList = new CustomListBox<SpellListBoxItem>(Width - 61, Height - 6, scrollBar)
            {
                Position = new Point(1, 5)
            };
            spellsList.SelectionChanged += spellsListBox_SelectedItemChanged;
            Add(spellsList);

            var textBoxTheme = new TextBoxTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255)),
                    Appearance_ControlFocused = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255))
                }
            };
            filterTextBox = new TextBox(Width - 69)
            {
                Position = new Point(10, 3),
                Theme = textBoxTheme,
                MaxLength = Width - 70
            };
            Add(filterTextBox);

            RefreshSpells();

            UpdateSpellDetails();
        }

        private void EditSelectedSpell()
        {
            var selectedSpell = spellsList.SelectedItem?.Spell;
            if (selectedSpell == null)
                return;

            var spellFilePath = EditSpellHelper.PrepareSpellTemplate(selectedSpell.Code);

            var editSpellView = new EditSpellView(spellFilePath)
            {
                Name = selectedSpell.Name,
                InitialManaCost = selectedSpell.ManaCost
            };
            editSpellView.Closed += (sender, args) =>
            {
                if (args.Result.HasValue && args.Result.Value == DialogResult.Cancel)
                    return;

                var code = EditSpellHelper.ReadSpellCodeFromFile(spellFilePath);
                var newSpell = new BookSpell
                {
                    Code = code,
                    Name = string.IsNullOrEmpty(editSpellView.Name) ? DefaultSpellName : editSpellView.Name,
                    ManaCost = editSpellView.ManaCost
                };
                libraryManager.RemoveSpell(selectedSpell);
                libraryManager.SaveSpell(newSpell);
                RefreshSpells();
            };

            editSpellView.Show();
        }

        private void RemoveSpellFromLibrary()
        {
            var selectedSpell = spellsList.SelectedItem?.Spell;
            if (selectedSpell == null)
                return;

            libraryManager.RemoveSpell(selectedSpell);
            RefreshSpells();
            spellsList.SelectedItemIndex = 0;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 3, "Filter:");

            Print(2, 1, "Spells Library");

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GetGlyph('┬'), FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╧'), FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, DefaultBackground));

            if (spellDetails.IsVisible)
            {
                Print(Width - 59, 4, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, DefaultBackground));
                Print(Width - 1, 4, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));
            }

            var newFilter = filterTextBox.EditingText ?? filterTextBox.Text;
            if (!string.Equals(newFilter, oldFilter))
            {
                oldFilter = newFilter;
                RefreshSpells();
            }
        }

        private BookSpell[] GetFilteredSpells()
        {
            if (string.IsNullOrEmpty(oldFilter))
                return GetSpells();

            return GetSpells().Where(spell => spell.Name.Contains(oldFilter)).ToArray();
        }

        private BookSpell[] GetSpells()
        {
            return libraryManager.ReadSpells();
        }

        private void RefreshSpells()
        {
            var selectedIndex = spellsList.SelectedItemIndex;

            spellsList.ClearItems();

            var spells = GetFilteredSpells();
            for (var index = 0; index < spells.Length; index++)
            {
                var spell = spells[index];
                spellsList.AddItem(new SpellListBoxItem(spell, index));
            }

            if (selectedIndex != -1)
            {
                spellsList.SelectedItemIndex = selectedIndex;
            }
            else
            {
                spellsList.SelectedItemIndex = 0;
            }
        }

        private void spellsListBox_SelectedItemChanged(object sender, EventArgs args)
        {
            UpdateSpellDetails();
        }

        private void UpdateSpellDetails()
        {
            var selectedSpellItem = spellsList.SelectedItem;
            spellDetails.IsVisible = selectedSpellItem != null;
            spellDetails.Spell = selectedSpellItem?.Spell;

            removeSpellButton.IsVisible = SelectedItem?.Spell != null;
            editSpellButton.IsVisible = SelectedItem?.Spell != null;
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.R:
                    RemoveSpellFromLibrary();
                    return true;
                case Keys.E:
                    EditSelectedSpell();
                    return true;
                case Keys.Up:
                case Keys.W:
                    MoveSelectionUp();
                    return true;
                case Keys.Down:
                case Keys.S:
                    MoveSelectionDown();
                    return true;
                case Keys.Escape:
                    Close();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        private void MoveSelectionUp()
        {
            spellsList.SelectedItemIndex = Math.Max(0, spellsList.SelectedItemIndex - 1);
        }

        private void MoveSelectionDown()
        {
            spellsList.SelectedItemIndex = Math.Min(spellsList.Items.Length - 1, spellsList.SelectedItemIndex + 1);
        }
    }
}