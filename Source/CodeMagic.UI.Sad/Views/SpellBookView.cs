using System;
using CodeMagic.Game.Items;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Orientation = SadConsole.Orientation;
using Point = Microsoft.Xna.Framework.Point;
using ScrollBar = SadConsole.Controls.ScrollBar;

namespace CodeMagic.UI.Sad.Views
{
    public class SpellBookView : GameViewBase, ISpellBookView
    {
        private StandardButton closeButton;
        private CustomListBox<SpellListBoxItem> spellsListBox;
        private SpellDetailsControl spellDetails;

        private StandardButton editSpellButton;
        private StandardButton castSpellButton;
        private StandardButton removeSpellButton;
        private StandardButton scribeSpellButton;
        private StandardButton saveToLibraryButton;
        private StandardButton loadFromLibraryButton;

        public void Initialize()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(closeButton);

            editSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 13),
                Text = "[E] Edit Spell"
            };
            editSpellButton.Click += (sender, args) => EditSpell?.Invoke(this, EventArgs.Empty);
            Add(editSpellButton);

            castSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 16),
                Text = "[C] Cast Spell"
            };
            castSpellButton.Click += (sender, args) => CastSpell?.Invoke(this, EventArgs.Empty);
            Add(castSpellButton);

            removeSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 19),
                Text = "[R] Remove Spell"
            };
            removeSpellButton.Click += (sender, args) => RemoveSpell?.Invoke(this, EventArgs.Empty);
            Add(removeSpellButton);

            scribeSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 22),
                Text = "[G] Write Scroll"
            };
            scribeSpellButton.Click += (sender, args) => ScribeSpell?.Invoke(this, EventArgs.Empty);
            Add(scribeSpellButton);

            saveToLibraryButton = new StandardButton(25)
            {
                Position = new Point(Width - 30, 16),
                Text = "[T] Save to Library"
            };
            saveToLibraryButton.Click += (sender, args) => SaveToLibrary?.Invoke(this, EventArgs.Empty);
            Add(saveToLibraryButton);

            loadFromLibraryButton = new StandardButton(25)
            {
                Position = new Point(Width - 30, 13),
                Text = "[L] Load from Library"
            };
            loadFromLibraryButton.Click += (sender, args) => LoadFromLibrary?.Invoke(this, EventArgs.Empty);
            Add(loadFromLibraryButton);

            spellDetails = new SpellDetailsControl(57, Height - 10, PlayerMana)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(spellDetails);

            var scrollBarTheme = new ScrollBarTheme
            {
                Normal = new Cell(DefaultForeground, DefaultBackground)
            };
            var scrollBar = new ScrollBar(Orientation.Vertical, Height - 4)
            {
                Position = new Point(Width - 60, 3),
                Theme = scrollBarTheme
            };
            Add(scrollBar);
            spellsListBox = new CustomListBox<SpellListBoxItem>(Width - 61, Height - 4, scrollBar)
            {
                Position = new Point(1, 3)
            };
            spellsListBox.SelectionChanged += spellsListBox_SelectedItemChanged;
            Add(spellsListBox);

            RefreshSpells();

            UpdateSpellDetails();
        }

        private void spellsListBox_SelectedItemChanged(object sender, EventArgs args)
        {
            UpdateSpellDetails();
        }

        private void UpdateSpellDetails()
        {
            var selectedSpellItem = spellsListBox.SelectedItem;
            spellDetails.IsVisible = selectedSpellItem != null;
            editSpellButton.IsVisible = selectedSpellItem != null;
            spellDetails.Spell = selectedSpellItem?.Spell;

            var spellExists = selectedSpellItem?.Spell != null;
            saveToLibraryButton.IsVisible = spellExists;
            removeSpellButton.IsVisible = spellExists;
            castSpellButton.IsVisible = spellExists;
            scribeSpellButton.IsVisible = spellExists && CanScribe;
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 1, "Spell Book:");
            surface.Print(14, 1, new ColoredString(SpellBook.Name, ItemDrawingHelper.GetItemColor(SpellBook).ToXna(), DefaultBackground));

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

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.E:
                    EditSpell?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.R:
                    RemoveSpell?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.C:
                    CastSpell?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.G:
                    ScribeSpell?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.T:
                    SaveToLibrary?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.L:
                    LoadFromLibrary?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.Up:
                case Keys.W:
                    MoveSelectionUp();
                    return true;
                case Keys.Down:
                case Keys.S:
                    MoveSelectionDown();
                    return true;
                default:
                    return false;
            }
        }

        private void MoveSelectionUp()
        {
            spellsListBox.SelectedItemIndex = Math.Max(0, spellsListBox.SelectedItemIndex - 1);
        }

        private void MoveSelectionDown()
        {
            spellsListBox.SelectedItemIndex = Math.Min(spellsListBox.Items.Length - 1, spellsListBox.SelectedItemIndex + 1);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler Exit;
        public event EventHandler SaveToLibrary;
        public event EventHandler LoadFromLibrary;
        public event EventHandler RemoveSpell;
        public event EventHandler CastSpell;
        public event EventHandler ScribeSpell;
        public event EventHandler EditSpell;

        public int SelectedSpellIndex => spellsListBox.SelectedItem.BookIndex;

        public SpellBook SpellBook { private get; set; }

        public BookSpell SelectedSpell => spellsListBox.SelectedItem?.Spell;

        public int? PlayerMana { private get; set; }

        public bool CanScribe { private get; set; }

        public void RefreshSpells()
        {
            var selectedIndex = spellsListBox.SelectedItemIndex;

            spellsListBox.ClearItems();

            for (var index = 0; index < SpellBook.Spells.Length; index++)
            {
                var spell = SpellBook.Spells[index];
                spellsListBox.AddItem(new SpellListBoxItem(spell, index));
            }

            if (selectedIndex != -1)
            {
                spellsListBox.SelectedItemIndex = selectedIndex;
            }
            else
            {
                spellsListBox.SelectedItemIndex = 0;
            }
        }
    }

    public class SpellListBoxItem : ICustomListBoxItem
    {
        private const string EmptySpellName = "Empty";
        private static readonly Color EmptySpellNameColor = Color.Gray;
        private static readonly Color SpellNameColor = Color.White;
        private static readonly Color SpellIndexColor = Color.Green;
        private static readonly Color SelectedItemBackColor = Color.FromNonPremultiplied(255, 128, 0, 255);
        private static readonly Color DefaultBackColor = Color.Black;

        public SpellListBoxItem(BookSpell spell, int bookIndex)
        {
            Spell = spell;
            BookIndex = bookIndex;
        }

        public BookSpell Spell { get; }

        public int BookIndex { get; }

        public bool Equals(ICustomListBoxItem other)
        {
            return ReferenceEquals(this, other);
        }

        public void Draw(CellSurface surface, int y, int maxWidth, bool selected)
        {
            var backColor = selected ? SelectedItemBackColor : DefaultBackColor;

            surface.Fill(0, y, maxWidth, null, backColor, null);

            var indexText = BookIndex < 9 ? $"0{BookIndex + 1}" : $"{BookIndex + 1}";
            surface.Print(1, y, indexText, new Cell(SpellIndexColor, backColor));

            var spellName = Spell?.Name ?? EmptySpellName;
            var spellNameColor = Spell == null ? EmptySpellNameColor : SpellNameColor;
            surface.Print(4, y, spellName, new Cell(spellNameColor, backColor));

            var manaCost = GetManaCostText();
            surface.Print(maxWidth - 5, y, manaCost, new Cell(TextHelper.ManaColor.ToXna(), backColor));
        }

        private string GetManaCostText()
        {
            if (Spell == null)
                return string.Empty;

            if (Spell.ManaCost >= 1000)
                return Spell.ManaCost.ToString();
            if (Spell.ManaCost >= 100)
                return $" {Spell.ManaCost}";
            if (Spell.ManaCost >= 10)
                return $"  {Spell.ManaCost}";
            return $"   {Spell.ManaCost}";
        }
    }
}