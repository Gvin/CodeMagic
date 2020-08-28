using System;
using CodeMagic.Game.Items;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Mono.Controls;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class SpellBookView : BaseWindow, ISpellBookView
    {
        private ListBox<SpellListBoxItem> spellsListBox;
        private SpellDetailsControl spellDetails;

        private FramedButton editSpellButton;
        private FramedButton castSpellButton;
        private FramedButton removeSpellButton;
        private FramedButton scribeSpellButton;
        private FramedButton saveToLibraryButton;
        private FramedButton loadFromLibraryButton;

        public SpellBookView() 
            : base(FontTarget.Interface)
        {
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

        public void Initialize()
        {
            var closeButton = new FramedButton(new Rectangle(Width - 17, Height - 4, 15, 3))
            {
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(closeButton);

            const int spellButtonWidth = 25;
            var spellButtonsX = Width - 57;

            editSpellButton = new FramedButton(new Rectangle(spellButtonsX, 13, spellButtonWidth, 3))
            {
                Text = "[E] Edit Spell"
            };
            editSpellButton.Click += (sender, args) => EditSpell?.Invoke(this, EventArgs.Empty);
            Controls.Add(editSpellButton);

            castSpellButton = new FramedButton(new Rectangle(spellButtonsX, 16, spellButtonWidth, 3))
            {
                Text = "[C] Cast Spell"
            };
            castSpellButton.Click += (sender, args) => CastSpell?.Invoke(this, EventArgs.Empty);
            Controls.Add(castSpellButton);

            removeSpellButton = new FramedButton(new Rectangle(spellButtonsX, 19, spellButtonWidth, 3))
            {
                Text = "[R] Remove Spell"
            };
            removeSpellButton.Click += (sender, args) => RemoveSpell?.Invoke(this, EventArgs.Empty);
            Controls.Add(removeSpellButton);

            scribeSpellButton = new FramedButton(new Rectangle(spellButtonsX, 22, spellButtonWidth, 3))
            {
                Text = "[G] Write Scroll"
            };
            scribeSpellButton.Click += (sender, args) => ScribeSpell?.Invoke(this, EventArgs.Empty);
            Controls.Add(scribeSpellButton);

            saveToLibraryButton = new FramedButton(new Rectangle(Width - 30, 16, spellButtonWidth, 3))
            {
                Text = "[T] Save to Library"
            };
            saveToLibraryButton.Click += (sender, args) => SaveToLibrary?.Invoke(this, EventArgs.Empty);
            Controls.Add(saveToLibraryButton);

            loadFromLibraryButton = new FramedButton(new Rectangle(Width - 30, 13, spellButtonWidth, 3))
            {
                Text = "[L] Load from Library"
            };
            loadFromLibraryButton.Click += (sender, args) => LoadFromLibrary?.Invoke(this, EventArgs.Empty);
            Controls.Add(loadFromLibraryButton);

            spellDetails = new SpellDetailsControl(new Rectangle(Width - 58, 3, 57, Height - 10), PlayerMana);
            Controls.Add(spellDetails);

            var scrollBar = new VerticalScrollBar(new Point(Width - 60, 3), Height - 4);
            Controls.Add(scrollBar);
            spellsListBox = new ListBox<SpellListBoxItem>(new Rectangle(1, 3, Width - 61, Height - 4), scrollBar);
            spellsListBox.SelectionChanged += spellsListBox_SelectedItemChanged;
            Controls.Add(spellsListBox);

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
            spellDetails.Visible = selectedSpellItem != null;
            editSpellButton.Visible = selectedSpellItem != null;
            spellDetails.Spell = selectedSpellItem?.Spell;

            var spellExists = selectedSpellItem?.Spell != null;
            saveToLibraryButton.Visible = spellExists;
            removeSpellButton.Visible = spellExists;
            castSpellButton.Visible = spellExists;
            scribeSpellButton.Visible = spellExists && CanScribe;
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Write(2, 1, "Spell Book:");
            surface.Write(14, 1, new ColoredString(SpellBook.Name, ItemDrawingHelper.GetItemColor(SpellBook).ToXna()));

            surface.Fill(new Rectangle(1, 2, Width - 2, 1), new Cell('─', FrameColor));
            surface.SetCell(0, 2, '╟', FrameColor);
            surface.SetCell(Width - 1, 2, '╢', FrameColor);

            surface.SetCell(Width - 59, 2, '┬', FrameColor);
            surface.SetCell(Width - 59, Height - 1, '╧', FrameColor);
            surface.Fill(new Rectangle(Width - 59, 3, 1, Height - 4), new Cell('│', FrameColor));

            if (spellDetails.Visible)
            {
                surface.SetCell(Width - 59, 4, '├', FrameColor);
                surface.SetCell(Width - 1, 4, '╢', FrameColor);
            }
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
                }
            }

            return base.ProcessKeysPressed(keys);
        }

        private void MoveSelectionUp()
        {
            spellsListBox.SelectedItemIndex = Math.Max(0, spellsListBox.SelectedItemIndex - 1);
        }

        private void MoveSelectionDown()
        {
            spellsListBox.SelectedItemIndex = Math.Min(spellsListBox.Items.Length - 1, spellsListBox.SelectedItemIndex + 1);
        }
    }

    public class SpellListBoxItem : IListBoxItem
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

        public bool Equals(IListBoxItem other)
        {
            return ReferenceEquals(this, other);
        }

        public void Draw(ICellSurface surface, int y, int maxWidth, bool selected)
        {
            var backColor = selected ? SelectedItemBackColor : DefaultBackColor;

            surface.Fill(new Rectangle(0, y, maxWidth, 1),  new Cell(' ', backColor: backColor));

            var indexText = BookIndex < 9 ? $"0{BookIndex + 1}" : $"{BookIndex + 1}";
            surface.Write(1, y, indexText, SpellIndexColor, backColor);

            var spellName = Spell?.Name ?? EmptySpellName;
            var spellNameColor = Spell == null ? EmptySpellNameColor : SpellNameColor;
            surface.Write(4, y, spellName, spellNameColor, backColor);

            var manaCost = GetManaCostText();
            surface.Write(maxWidth - 5, y, manaCost, TextHelper.ManaColor.ToXna(), backColor);
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