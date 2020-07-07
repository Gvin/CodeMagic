using System;
using System.Linq;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Orientation = SadConsole.Orientation;
using ScrollBar = SadConsole.Controls.ScrollBar;
using TextBox = SadConsole.Controls.TextBox;

namespace CodeMagic.UI.Sad.Views
{
    public abstract class SpellsLibraryViewBase : GameViewBase
    {
        private CustomListBox<SpellListBoxItem> spellsList;
        private SpellDetailsControl spellDetails;
        private TextBox filterTextBox;

        private StandardButton removeSpellButton;
        private StandardButton editSpellButton;
        private string filter;

        protected SpellsLibraryViewBase()
        {
            InitializeControls();
        }

        public BookSpell SelectedSpell => SelectedItem?.Spell;

        private SpellListBoxItem SelectedItem => spellsList.SelectedItem;

        public BookSpell[] Spells { get; set; }

        public event EventHandler RemoveSpell;

        private void InitializeControls()
        {
            removeSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 16),
                Text = "[R] Remove from Library"
            };
            removeSpellButton.Click += (sender, args) => RemoveSpell?.Invoke(this, EventArgs.Empty);
            Add(removeSpellButton);

            editSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 13),
                Text = "[E] Edit Spell"
            };
            editSpellButton.Click += (sender, args) => EditSpell?.Invoke(this, EventArgs.Empty);
            Add(editSpellButton);

            spellDetails = new SpellDetailsControl(57, Height - 10)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(spellDetails);

            var scrollBarTheme = new ScrollBarTheme
            {
                Normal = new Cell(DefaultForeground, DefaultBackground)
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
                Normal = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255)),
                Focused = new Cell(Color.White, Color.FromNonPremultiplied(66, 66, 66, 255))
            };
            filterTextBox = new TextBox(Width - 69)
            {
                Position = new Point(10, 3),
                Theme = textBoxTheme,
                MaxLength = Width - 70
            };
            Add(filterTextBox);

            UpdateSpellDetails();
        }

        public event EventHandler EditSpell;

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 3, "Filter:");

            surface.Print(2, 1, "Spells Library");

            surface.Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            surface.Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            surface.Print(Width - 59, 2, new ColoredGlyph(Glyphs.GetGlyph('┬'), FrameColor, DefaultBackground));
            surface.Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╧'), FrameColor, DefaultBackground));
            surface.DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, DefaultBackground));

            if (spellDetails.IsVisible)
            {
                surface.Print(Width - 59, 4, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, DefaultBackground));
                surface.Print(Width - 1, 4, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));
            }
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            var newFilter = filterTextBox.EditingText ?? filterTextBox.Text;
            if (!string.Equals(newFilter, filter))
            {
                filter = newFilter;
                RefreshSpells(false);
            }
        }

        private BookSpell[] GetFilteredSpells()
        {
            if (string.IsNullOrEmpty(filter))
                return Spells;

            return Spells.Where(spell => spell.Name.Contains(filter)).ToArray();
        }

        public void RefreshSpells(bool keepSelection)
        {
            var selectedIndex = keepSelection ? spellsList.SelectedItemIndex : 0;

            spellsList.ClearItems();

            for (var index = 0; index < GetFilteredSpells().Length; index++)
            {
                var spell = Spells[index];
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
                    RemoveSpell?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.E:
                    EditSpell?.Invoke(this, EventArgs.Empty);
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
                    OnExit();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        public event EventHandler Exit;

        protected void OnExit()
        {
            Exit?.Invoke(this, EventArgs.Empty);
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