using System;
using System.Drawing;
using System.IO;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Console.Controls;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Views
{
    public class SpellBookView : View
    {
        private const int SpellNameMaxWidth = 60;

        private readonly GameCore game;

        private VerticalMenuControl<BookSpell> spellsMenu;
        private SpellDetailsPanel spellDetailsPanel;

        public SpellBookView(GameCore game)
        {
            this.game = game;

            InitializeControls();
        }

        private void InitializeControls()
        {
            spellDetailsPanel = new SpellDetailsPanel
            {
                X = 80,
                Y = 3,
                Width = 40,
                Height = Writer.ScreenHeight - 5
            };
            Controls.Add(spellDetailsPanel);

            spellsMenu = new VerticalMenuControl<BookSpell>
            {
                X = 2,
                Y = 5,
                ItemWidth = 60,
                TextColor = Color.Lime,
                SelectedFrameColor = Color.Yellow
            };

            RebuildSpellsMenu();

            spellsMenu.SelectedItemIndex = 0;
            spellsMenu.SelectionChanged += spellsMenu_SelectionChanged;

            Controls.Add(spellsMenu);
        }

        private void spellsMenu_SelectionChanged(object sender, EventArgs args)
        {
            spellDetailsPanel.Spell = spellsMenu.SelectedItem.Data;
        }

        private string GetSpellName(BookSpell spell, int index)
        {
            var initialName = spell?.Name ?? "Empty";
            var indexText = index < 10 ? $"0{index}" : index.ToString();
            return $" {indexText} - {initialName}";
        }

        private SpellBook Book => game.Player.Equipment.SpellBook;

        public override void DrawStatic()
        {
            Writer.CursorY = 2;
            Writer.CursorX = 5;
            Writer.Write("Spell book: ", Color.White);
            Writer.WriteLine(Book.Name, ItemDrawingHelper.GetItemNameColor(Book));
            Writer.WriteAt(0, 3, LineTypes.DoubleVerticalSingleRight, Color.Gray);
            Writer.DrawHorizontalLine(3, 1, Writer.ScreenWidth - 2, LineTypes.SingleHorizontal, Color.Gray);
            Writer.WriteAt(Writer.ScreenWidth - 1, 3, LineTypes.DoubleVerticalSingleLeft, Color.Gray);

            base.DrawStatic();
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    Close();
                    break;
                case ConsoleKey.C:
                    ProcessCKey();
                    break;
                case ConsoleKey.E:
                    ProcessEKey();
                    break;
                case ConsoleKey.R:
                    ProcessRKey();
                    break;
            }
        }

        private void ProcessCKey()
        {
            var spell = spellsMenu.SelectedItem?.Data;
            if (spell != null)
            {
                game.PerformPlayerAction(new CastSpellAction(spell));
                Close();
            }
        }

        private void ProcessEKey()
        {
            CreateOrEditSpell();
        }

        private void ProcessRKey()
        {
            Book.Spells[spellsMenu.SelectedItemIndex] = null;
            RefreshSpellsMenu();
        }

        private void CreateOrEditSpell()
        {
            var spell = spellsMenu.SelectedItem?.Data;
            var spellFilePath = $"{Path.GetTempFileName()}.js";

            if (spell == null)
            {
                var spellTemplate = File.ReadAllText(".\\Resources\\Templates\\SpellTemplate.js");
                File.WriteAllText(spellFilePath, spellTemplate);
            }
            else
            {
                File.WriteAllText(spellFilePath, spell.Code);
            }

            var waitDialog = new WaitForEditFinishView();
            waitDialog.Closing += (sender, args) =>
            {
                var spellDetailsView = new EnterSpellDataView();
                spellDetailsView.InitialManaLevel = spell?.ManaCost;
                spellDetailsView.Name = spell?.Name;

                spellDetailsView.Closed += (sender2, args2) =>
                {
                    var fileContent = File.ReadAllText(spellFilePath);
                    File.Delete(spellFilePath);
                    var trimmedName = spellDetailsView.Name.Length > SpellNameMaxWidth
                        ? spellDetailsView.Name.Substring(0, SpellNameMaxWidth)
                        : spellDetailsView.Name;
                    var newSpell = new BookSpell { Code = fileContent, ManaCost = spellDetailsView.ManaLevel, Name = trimmedName };
                    Book.Spells[spellsMenu.SelectedItemIndex] = newSpell;
                    RefreshSpellsMenu();
                };
                spellDetailsView.Show();
            };
            waitDialog.Show();

            StartEditor(spellFilePath);
        }

        private void RebuildSpellsMenu()
        {
            spellsMenu.Items.Clear();
            for (var index = 0; index < Book.Spells.Length; index++)
            {
                var spell = Book.Spells[index];
                var item = new SpellMenuItem(GetSpellName(spell, index + 1), spell);
                spellsMenu.Items.Add(item);
            }
        }

        private void RefreshSpellsMenu()
        {
            var storedIndex = spellsMenu.SelectedItemIndex;
            RebuildSpellsMenu();
            spellsMenu.SelectedItemIndex = storedIndex;
        }

        private void StartEditor(string spellFilePath)
        {
            var editorPath = Properties.Settings.Default.EditingToolPath;
            if (string.IsNullOrEmpty(editorPath))
            {
                System.Diagnostics.Process.Start(spellFilePath);
            }
            else
            {
                System.Diagnostics.Process.Start(editorPath, spellFilePath);
            }
        }

        private class SpellMenuItem : MenuItem<BookSpell>
        {
            public SpellMenuItem(string text, BookSpell data) 
                : base(text, data)
            {
            }

            public override void WriteText(int x, int y, int maxLength, Color textColor, IControlWriter writer)
            {
                if (Data == null)
                {
                    base.WriteText(x, y, maxLength, textColor, writer);
                    return;
                }

                var manaText = $" {Data.ManaCost} ";
                var nameText = CutText(Text, maxLength - manaText.Length);

                writer.WriteAt(x, y, nameText, textColor, Color.Black);
                writer.Write(manaText, Color.Blue, Color.Black);
            }
        }
    }
}