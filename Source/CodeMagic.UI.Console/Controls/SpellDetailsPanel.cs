using System.Drawing;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public class SpellDetailsPanel : ConsoleControl
    {
        public BookSpell Spell { get; set; }

        protected override void DrawStatic(IControlWriter writer)
        {
            base.DrawStatic(writer);

            writer.WriteAt(0, 0, LineTypes.SingleHorizontalDown, Color.Gray);
            writer.DrawVerticalLine(0, 1, Height - 1, false, Color.Gray);
            writer.WriteAt(0, Height, LineTypes.DoubleHorizontalSingleUp, Color.Gray);

            writer.CursorX = 2;
            writer.CursorY = 2;

            writer.WriteLine("Selected spell details:", Color.White);
            writer.DrawHorizontalLine(4, 0, Width - 2, false, Color.Gray);
            writer.WriteAt(Width - 1, 4, LineTypes.DoubleVerticalSingleLeft, Color.Gray);
            writer.WriteAt(0, 4, LineTypes.SingleVerticalRight, Color.Gray);
        }

        protected override void DrawDynamic(IControlWriter writer)
        {
            base.DrawDynamic(writer);

            
            writer.CursorY = 7;
            writer.CursorX = 2;

            if (Spell == null)
            {
                writer.WriteLine("Spell not selected", Color.DarkGray);
            }
            else
            {
                writer.Write("Mana Cost: ", Color.White);
                writer.WriteLine($"{Spell.ManaCost}            ", Color.Blue);
            }

            writer.CursorY = 11;
            writer.CursorX = 2;
            writer.WriteLine("Actions:", Color.White);
            writer.CursorX = 2;

            writer.WriteLine("[E] - Edit", Color.White);
            writer.CursorX = 2;
            if (Spell == null)
            {
                writer.WriteLine("              ", Color.White);
                writer.CursorX = 2;
                writer.WriteLine("              ", Color.White);
                writer.CursorX = 2;
                writer.WriteLine("              ", Color.White);
            }
            else
            {
                writer.WriteLine("[R] - Remove", Color.White);
                writer.CursorX = 2;
                writer.WriteLine("[C] - Cast", Color.White);
            }
        }
    }
}