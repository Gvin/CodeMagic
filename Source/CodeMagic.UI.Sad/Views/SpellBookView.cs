using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Sad.SpellsLibrary;
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
    public class SpellBookView : GameViewBase
    {
        private const string DefaultSpellName = "New Spell";

        private readonly GameCore<Player> game;

        private StandardButton closeButton;
        private CustomListBox<SpellListBoxItem> spellsListBox;
        private SpellDetailsControl spellDetails;

        private StandardButton editSpellButton;
        private StandardButton castSpellButton;
        private StandardButton removeSpellButton;
        private StandardButton scribeSpellButton;
        private StandardButton saveToLibraryButton;
        private StandardButton loadFromLibraryButton;

        public SpellBookView(GameCore<Player> game)
        {
            this.game = game;

            InitializeControls();
        }

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += closeButton_Click;
            Add(closeButton);

            editSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 13),
                Text = "[E] Edit Spell"
            };
            editSpellButton.Click += editSpellButton_Click;
            Add(editSpellButton);

            castSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 16),
                Text = "[C] Cast Spell"
            };
            castSpellButton.Click += castSpellButton_Click;
            Add(castSpellButton);

            removeSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 19),
                Text = "[R] Remove Spell"
            };
            removeSpellButton.Click += removeSpellButton_Click;
            Add(removeSpellButton);

            scribeSpellButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 22),
                Text = "[G] Write Scroll"
            };
            scribeSpellButton.Click += scribeSpellButton_Click;
            Add(scribeSpellButton);

            saveToLibraryButton = new StandardButton(25)
            {
                Position = new Point(Width - 30, 16),
                Text = "[T] Save to Library"
            };
            saveToLibraryButton.Click += (sender, args) => SaveSpellToLibrary();
            Add(saveToLibraryButton);

            loadFromLibraryButton = new StandardButton(25)
            {
                Position = new Point(Width - 30, 13),
                Text = "[L] Load from Library"
            };
            loadFromLibraryButton.Click += (sender, args) => LoadSpellFromLibrary();
            Add(loadFromLibraryButton);

            spellDetails = new SpellDetailsControl(57, Height - 10)
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

        private void SaveSpellToLibrary()
        {
            var selectedSpell = spellsListBox.SelectedItem?.Spell;
            if (selectedSpell == null)
                return;

            new SpellsLibraryManager().SaveSpell(selectedSpell);
        }

        private void LoadSpellFromLibrary()
        {
            var view = new LoadSpellFromLibraryView();
            view.Closed += (sender, args) =>
            {
                if (args.Result == DialogResult.Ok && view.SelectedSpell != null)
                {
                    Book.Spells[spellsListBox.SelectedItem.BookIndex] = view.SelectedSpell;
                    RefreshSpells();
                }
            };
            view.Show();
        }

        private void scribeSpellButton_Click(object sender, EventArgs e)
        {
            WriteSpellToScroll();
        }

        private void WriteSpellToScroll()
        {
            var selectedSpell = spellsListBox.SelectedItem?.Spell;
            if (selectedSpell == null)
                return;

            var blankScroll = game.Player.Inventory.GetItem(BlankScroll.ItemKey);
            if (blankScroll == null)
                return;

            var scrollCreationCost = selectedSpell.ManaCost * 2;
            if (game.Player.Mana < scrollCreationCost)
            {
                game.Journal.Write(new NotEnoughManaToScrollMessage());
                return;
            }

            game.Player.Mana -= scrollCreationCost;

            game.Player.Inventory.RemoveItem(blankScroll);
            var newScroll = new Scroll(new ScrollItemConfiguration
            {
                Name = $"{selectedSpell.Name} Scroll ({selectedSpell.ManaCost})",
                Key = Guid.NewGuid().ToString(),
                Weight = 1,
                Code = selectedSpell.Code,
                SpellName = selectedSpell.Name,
                Mana = selectedSpell.ManaCost,
                Rareness = ItemRareness.Uncommon
            });
            game.Player.Inventory.AddItem(newScroll);

            Close();
            game.PerformPlayerAction(new EmptyPlayerAction());
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
            var selectedSpellItem = spellsListBox.SelectedItem;
            if (selectedSpellItem == null)
                return;

            var spellFilePath = EditSpellHelper.PrepareSpellTemplate(selectedSpellItem.Spell?.Code);

            var editSpellView = new EditSpellView(spellFilePath)
            {
                Name = selectedSpellItem.Spell?.Name,
                InitialManaCost = selectedSpellItem.Spell?.ManaCost
            };
            editSpellView.Closed += (sender, args) =>
            {
                if (args.Result == DialogResult.Cancel)
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
            var selectedSpellItem = spellsListBox.SelectedItem;
            if (selectedSpellItem?.Spell == null)
                return;

            Book.Spells[selectedSpellItem.BookIndex] = null;
            RefreshSpells();
        }

        private void CastSelectedSpell()
        {
            var selectedSpellItem = spellsListBox.SelectedItem;
            if (selectedSpellItem?.Spell == null)
                return;

            Close();
            game.PerformPlayerAction(new CastSpellPlayerAction(selectedSpellItem.Spell));
        }

        private void RefreshSpells()
        {
            var selectedIndex = spellsListBox.SelectedItemIndex;

            spellsListBox.ClearItems();

            for (var index = 0; index < Book.Spells.Length; index++)
            {
                var spell = Book.Spells[index];
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
            scribeSpellButton.IsVisible = spellExists && game.Player.Inventory.Contains(BlankScroll.ItemKey);
        }

        private SpellBook Book => game.Player.Equipment.SpellBook;

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(2, 1, "Spell Book:");
            surface.Print(14, 1, new ColoredString(Book.Name, ItemDrawingHelper.GetItemColor(Book).ToXna(), DefaultBackground));

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
                case Keys.G:
                    WriteSpellToScroll();
                    return true;
                case Keys.T:
                    SaveSpellToLibrary();
                    return true;
                case Keys.L:
                    LoadSpellFromLibrary();
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

        private void closeButton_Click(object sender, EventArgs args)
        {
            Close();
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