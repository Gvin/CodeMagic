using System;
using System.Linq;
using CodeMagic.Game.Spells;
using CodeMagic.UI.Mono.Controls;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public abstract class SpellsLibraryViewBase : BaseWindow
    {
        private readonly ListBox<SpellListBoxItem> spellsList;
        private readonly SpellDetailsControl spellDetails;
        //private TextBox filterTextBox;

        private readonly FramedButton removeSpellButton;
        private readonly FramedButton editSpellButton;
        private string filter;

        public BookSpell SelectedSpell => SelectedItem?.Spell;

        private SpellListBoxItem SelectedItem => spellsList.SelectedItem;

        public BookSpell[] Spells { get; set; }

        public event EventHandler RemoveSpell;
        public event EventHandler EditSpell;
        public event EventHandler Exit;

        protected SpellsLibraryViewBase() : base(FontTarget.Interface)
        {
            removeSpellButton = new FramedButton(new Rectangle(Width - 57, 16, 25, 3))
            {
                Text = "[R] Remove from Library"
            };
            removeSpellButton.Click += (sender, args) => RemoveSpell?.Invoke(this, EventArgs.Empty);
            Controls.Add(removeSpellButton);

            editSpellButton = new FramedButton(new Rectangle(Width - 57, 13, 25, 3))
            {
                Text = "[E] Edit Spell"
            };
            editSpellButton.Click += (sender, args) => EditSpell?.Invoke(this, EventArgs.Empty);
            Controls.Add(editSpellButton);

            spellDetails = new SpellDetailsControl(new Rectangle(Width - 58, 3, 57, Height - 10));
            Controls.Add(spellDetails);

            var scrollBar = new VerticalScrollBar(new Point(Width - 60, 5), Height - 6);
            Controls.Add(scrollBar);
            spellsList = new ListBox<SpellListBoxItem>(new Rectangle(1, 5, Width - 61, Height - 6), scrollBar);
            spellsList.SelectionChanged += spellsListBox_SelectedItemChanged;
            Controls.Add(spellsList);

            // filterTextBox = new TextBox(Width - 69)
            // {
            //     Position = new Point(10, 3),
            //     Theme = textBoxTheme,
            //     MaxLength = Width - 70
            // };
            // Add(filterTextBox);

            UpdateSpellDetails();
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
            spellDetails.Visible = selectedSpellItem != null;
            spellDetails.Spell = selectedSpellItem?.Spell;

            removeSpellButton.Visible = SelectedItem?.Spell != null;
            editSpellButton.Visible = SelectedItem?.Spell != null;
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Write(2, 3, "Filter:");

            surface.Write(2, 1, "Spells Library");

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

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            // var newFilter = filterTextBox.EditingText ?? filterTextBox.Text;
            // if (!string.Equals(newFilter, filter))
            // {
            //     filter = newFilter;
            //     RefreshSpells(false);
            // }
        }

        public override bool ProcessKeyPressed(Keys key)
        {
            switch (key)
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