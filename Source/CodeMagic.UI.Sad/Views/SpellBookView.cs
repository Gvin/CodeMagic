using System;
using System.Windows.Forms;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.GameProcess;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Button = SadConsole.Controls.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ListBox = SadConsole.Controls.ListBox;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class SpellBookView : View
    {
        private const string DefaultSpellName = "New Spell";

        private readonly IGameCore game;

        private Button closeButton;
        private ListBox spellsListBox;
        private SpellDetailsControl spellDetails;

        private Button editSpellButton;
        private Button castSpellButton;
        private Button removeSpellButton;

        public SpellBookView(IGameCore game) 
            : base(Program.Width, Program.Height)
        {
            this.game = game;

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
            closeButton.Click += closeButton_Click;
            Add(closeButton);

            spellDetails = new SpellDetailsControl(57, Height - 10)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(spellDetails);

            spellsListBox = new ListBox(Width - 60, Height - 6)
            {
                Position = new Point(1, 3),
                CompareByReference = true
            };
            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };
            var listBoxTheme = new ListBoxTheme(scrollBarTheme)
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground)
                }
            };
            spellsListBox.Theme = listBoxTheme;
            spellsListBox.SelectedItemChanged += spellsListBox_SelectedItemChanged;
            Add(spellsListBox);
            RefreshSpells();

            editSpellButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 13),
                Text = "[E] Edit Spell",
                CanFocus = false,
                Theme = buttonsTheme
            };
            editSpellButton.Click += editSpellButton_Click;
            Add(editSpellButton);

            castSpellButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 16),
                Text = "[C] Cast Spell",
                CanFocus = false,
                Theme = buttonsTheme
            };
            castSpellButton.Click += castSpellButton_Click;
            Add(castSpellButton);

            removeSpellButton = new Button(20, 3)
            {
                Position = new Point(Width - 57, 19),
                Text = "[R] Remove Spell",
                CanFocus = false,
                Theme = buttonsTheme
            };
            removeSpellButton.Click += removeSpellButton_Click;
            Add(removeSpellButton);

            UpdateSpellDetails();
        }

        private void castSpellButton_Click(object sender, EventArgs args)
        {
            CastSelectedSpell();
        }

        private void removeSpellButton_Click(object sender, EventArgs args)
        {
            RemoveSelectedSpell();
        }

        private void editSpellButton_Click(object sender, EventArgs args)
        {
            PerformSpellEdit();
        }

        private void PerformSpellEdit()
        {
            var selectedSpellItem = spellsListBox.SelectedItem as SpellListBoxItem;
            if (selectedSpellItem == null)
                return;

            var spellFilePath = EditSpellHelper.PrepareSpellTemplate(selectedSpellItem.Spell);

            var editSpellView = new EditSpellView(spellFilePath)
            {
                Name = selectedSpellItem.Spell?.Name,
                InitialManaCost = selectedSpellItem.Spell?.ManaCost
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
                Book.Spells[selectedSpellItem.BookIndex] = newSpell;
                RefreshSpells();
            };

            editSpellView.Show();
        }

        private void RemoveSelectedSpell()
        {
            var selectedSpellItem = spellsListBox.SelectedItem as SpellListBoxItem;
            if (selectedSpellItem?.Spell == null)
                return;

            Book.Spells[selectedSpellItem.BookIndex] = null;
            RefreshSpells();
        }

        private void CastSelectedSpell()
        {
            var selectedSpellItem = spellsListBox.SelectedItem as SpellListBoxItem;
            if (selectedSpellItem?.Spell == null)
                return;

            game.PerformPlayerAction(new CastSpellAction(selectedSpellItem.Spell));
            Close();
        }

        private void RefreshSpells()
        {
            var selectedIndex = spellsListBox.SelectedIndex;

            spellsListBox.Items.Clear();

            for (var index = 0; index < Book.Spells.Length; index++)
            {
                var spell = Book.Spells[index];
                spellsListBox.Items.Add(new SpellListBoxItem(spell, index));
            }

            if (selectedIndex != -1)
            {
                spellsListBox.SelectedItem = spellsListBox.Items[selectedIndex];
            }
        }

        private void spellsListBox_SelectedItemChanged(object sender, EventArgs args)
        {
            UpdateSpellDetails();
        }

        private void UpdateSpellDetails()
        {
            var selectedSpellItem = spellsListBox.SelectedItem as SpellListBoxItem;
            spellDetails.IsVisible = selectedSpellItem != null;
            editSpellButton.IsVisible = selectedSpellItem != null;
            spellDetails.Spell = selectedSpellItem?.Spell;

            var spellExists = selectedSpellItem?.Spell != null;
            removeSpellButton.IsVisible = spellExists;
            castSpellButton.IsVisible = spellExists;
        }

        private SpellBook Book => game.Player.Equipment.SpellBook;

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, "Spell Book:");
            Print(14, 1, new ColoredString(Book.Name, ItemDrawingHelper.GetItemColor(Book), DefaultBackground));

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GlyphBoxSingleHorizontal);
            Print(0, 2, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleRight, FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GlyphBoxSingleHorizontalDown, FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GlyphBoxDoubleHorizontalSingleUp, FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GlyphBoxSingleVertical, FrameColor, DefaultBackground));

            if (spellDetails.IsVisible)
            {
                Print(Width - 59, 4, new ColoredGlyph(Glyphs.GlyphBoxSingleVerticalRight, FrameColor, DefaultBackground));
                Print(Width - 1, 4, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, DefaultBackground));
            }
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Close();
                    return true;
                case Keys.E:
                    PerformSpellEdit();
                    return true;
                case Keys.R:
                    RemoveSelectedSpell();
                    return true;
                case Keys.C:
                    CastSelectedSpell();
                    return true;
                default:
                    return false;
            }
        }

        private void closeButton_Click(object sender, EventArgs args)
        {
            Close();
        }

        private class SpellListBoxItem
        {
            private const string EmptySpellName = "Empty";

            public SpellListBoxItem(BookSpell spell, int bookIndex)
            {
                Spell = spell;
                BookIndex = bookIndex;
            }

            public BookSpell Spell { get; }

            public int BookIndex { get; }

            public override string ToString()
            {
                var spellName = Spell?.Name ?? EmptySpellName;
                var indexText = BookIndex < 9 ? $"0{BookIndex + 1}" : $"{BookIndex + 1}";
                return $" {indexText} - {spellName}";
            }
        }
    }
}