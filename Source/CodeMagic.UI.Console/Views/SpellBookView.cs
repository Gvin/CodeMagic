using System;
using System.Drawing;
using System.IO;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Console.Drawing;
using Writer = Colorful.Console;

namespace CodeMagic.UI.Console.Views
{
    public class SpellBookView : View
    {
        private const int SpellDetailsLeftShift = 80;
        private const int SpellNameMaxWidth = 60;

        private readonly GameCore game;
        private int selectedSpellIndex;

        public SpellBookView(GameCore game)
        {
            this.game = game;
            selectedSpellIndex = 0;
        }

        private SpellBook Book => game.Player.Equipment.SpellBook;

        public override void DrawStatic()
        {
            base.DrawStatic();

            Writer.CursorTop = 2;
            Writer.CursorLeft = 5;
            Writer.Write("Spell book: ", Color.White);
            Writer.WriteLine(Book.Name, ItemDrawingHelper.GetItemNameColor(Book));
            DrawingHelper.WriteAt(LineTypes.DoubleVerticalAndSingleRight, 0, 3, Color.Gray);
            DrawingHelper.DrawHorizontalLine(3, 1, Writer.WindowWidth - 2, LineTypes.SingleHorizontal, Color.Gray);
            DrawingHelper.WriteAt(LineTypes.DoubleVerticalAndSingleLeft, Writer.WindowWidth - 1, 3, Color.Gray);
        }

        public override void DrawDynamic()
        {
            base.DrawDynamic();

            Writer.CursorTop = 5;

            for (var index = 0; index < Book.Spells.Length; index++)
            {
                var spell = Book.Spells[index];
                DrawSpell(spell, index);
            }

            DrawSpellDetails(Book.Spells[selectedSpellIndex]);
        }

        private void DrawSpell(Spell spell, int index)
        {
            Writer.CursorLeft = 5;

            var selected = selectedSpellIndex == index;

            if (selected)
            {
                Writer.Write(LineTypes.SingleDownRight, Color.Yellow);
                DrawingHelper.DrawHorizontalLine(Writer.CursorTop, 6, SpellNameMaxWidth + 5,
                    LineTypes.SingleHorizontal, Color.Yellow);
                Writer.Write(LineTypes.SingleDownLeft, Color.Yellow);
                Writer.CursorTop++;
            }
            else
            {
                for (var i = 0; i < SpellNameMaxWidth + 4; i++)
                {
                    Writer.Write(" ", Color.DarkGray);
                }

                Writer.CursorTop++;
            }

            Writer.CursorLeft = 2;
            Writer.Write($"{index + 1}", Color.LimeGreen);
            Writer.CursorLeft = 5;
            Writer.Write(selected ? LineTypes.SingleVertical : ' ', Color.Yellow);
            var spellName = spell?.Name ?? "Empty";
            while (spellName.Length < SpellNameMaxWidth)
                spellName += " ";
            Writer.Write(spellName, Color.LimeGreen);
            Writer.Write(selected ? LineTypes.SingleVertical : ' ', Color.Yellow);
            Writer.Write($" {spell?.ManaCost}", Color.Blue);
            Writer.CursorTop++;

            Writer.CursorLeft = 5;
            if (selected)
            {
                Writer.Write(LineTypes.SingleUpRight, Color.Yellow);
                DrawingHelper.DrawHorizontalLine(Writer.CursorTop, 6, SpellNameMaxWidth + 5,
                    LineTypes.SingleHorizontal, Color.Yellow);
                Writer.Write(LineTypes.SingleUpLeft, Color.Yellow);
                Writer.CursorTop++;
            }
            else
            {
                for (var i = 0; i < SpellNameMaxWidth + 2; i++)
                {
                    Writer.Write(" ", Color.DarkGray);
                }

                Writer.CursorTop++;
            }
        }

        private void DrawSpellDetails(Spell spell)
        {
            DrawingHelper.WriteAt(LineTypes.SingleHorizontalAndDown, SpellDetailsLeftShift - 1, 3, Color.Gray);
            DrawingHelper.DrawVerticalLine(SpellDetailsLeftShift - 1, 4, Writer.WindowHeight - 3, false, Color.Gray);
            DrawingHelper.WriteAt(LineTypes.DoubleHorizontalAndSingleUp, SpellDetailsLeftShift - 1, Writer.WindowHeight - 2, Color.Gray);

            Writer.CursorLeft = SpellDetailsLeftShift;
            Writer.CursorTop = 5;

            Writer.WriteLine("Selected spell details:", Color.White);
            DrawingHelper.DrawHorizontalLine(6, SpellDetailsLeftShift, Writer.WindowWidth - 2, false, Color.Gray);
            DrawingHelper.WriteAt(LineTypes.DoubleVerticalAndSingleLeft, Writer.WindowWidth - 1, 6, Color.Gray);
            DrawingHelper.WriteAt(LineTypes.SingleVerticalAndRight, SpellDetailsLeftShift - 1, 6, Color.Gray);
            Writer.CursorTop++;
            Writer.CursorLeft = SpellDetailsLeftShift;

            if (spell == null)
            {
                Writer.WriteLine("Spell not selected", Color.DarkGray);
            }
            else
            {
                Writer.Write("Mana Cost: ", Color.White);
                Writer.WriteLine($"{spell.ManaCost}            ", Color.Blue);
            }

            Writer.CursorLeft = SpellDetailsLeftShift;
            Writer.CursorTop = 11;
            Writer.CursorLeft = SpellDetailsLeftShift;
            Writer.WriteLine("Actions:", Color.White);
            Writer.CursorLeft = SpellDetailsLeftShift;

            Writer.WriteLine("[E] - Edit", Color.White);
            Writer.CursorLeft = SpellDetailsLeftShift;
            if (spell == null)
            {
                Writer.WriteLine("              ", Color.White);
                Writer.CursorLeft = SpellDetailsLeftShift;
                Writer.WriteLine("              ", Color.White);
                Writer.CursorLeft = SpellDetailsLeftShift;
                Writer.WriteLine("              ", Color.White);
            }
            else
            {
                Writer.WriteLine("[R] - Remove", Color.White);
                Writer.CursorLeft = SpellDetailsLeftShift;
                Writer.WriteLine("[C] - Cast", Color.White);
            }
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    Close();
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    selectedSpellIndex--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    selectedSpellIndex++;
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

            if (selectedSpellIndex < 0)
                selectedSpellIndex = Book.Size - 1;
            if (selectedSpellIndex >= Book.Size)
                selectedSpellIndex = 0;
        }

        private void ProcessCKey()
        {
            var spell = Book.Spells[selectedSpellIndex];
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
            Book.Spells[selectedSpellIndex] = null;
        }

        private void CreateOrEditSpell()
        {
            var spell = Book.Spells[selectedSpellIndex];
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
                    Book.Spells[selectedSpellIndex] = new Spell { Code = fileContent, ManaCost = spellDetailsView.ManaLevel, Name = trimmedName };
                };
                spellDetailsView.Show();
            };
            waitDialog.Show();

            StartEditor(spellFilePath);
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
    }
}