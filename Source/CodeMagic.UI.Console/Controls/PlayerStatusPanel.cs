using System.Drawing;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public class PlayerStatusPanel : ConsoleControl
    {
        private readonly IPlayer player;
        private readonly Color frameColor;

        public PlayerStatusPanel(IPlayer player, Color frameColor)
        {
            this.player = player;
            this.frameColor = frameColor;
        }

        protected override void DrawStatic(IControlWriter writer)
        {
            base.DrawStatic(writer);

            writer.WriteAt(0, 0, LineTypes.DoubleHorizontalSingleDown, frameColor);
            writer.DrawVerticalLine(0, 1, Height - 1, false, frameColor);

            writer.CursorY = 1;
            writer.CursorX = 2;
            writer.BackColor = Color.Black;

            writer.Write("Player Stats", Color.White);
            writer.DrawHorizontalLine(2, 1, Width - 1, false, frameColor);
            writer.Write(LineTypes.DoubleVerticalSingleLeft, frameColor);
            writer.WriteAt(0, 2, LineTypes.SingleVerticalRight, frameColor);


            writer.CursorY = 20;
            writer.CursorX = 2;
            writer.WriteLine("Actions:", Color.White);
            writer.CursorX = 2;
            writer.WriteLine("[F] - Mele Attack", Color.White);
            writer.CursorX = 2;
            writer.WriteLine("[C] - Spell Book", Color.White);
        }

        protected override void DrawDynamic(IControlWriter writer)
        {
            base.DrawDynamic(writer);

            writer.CursorX = 2;
            writer.CursorY = 4;

            writer.Write("HP:   ", Color.White);
            writer.Write($"{player.Health} / {player.MaxHealth}   ", Color.Red);

            writer.CursorY++;
            writer.CursorX = 2;
            writer.Write("Mana: ", Color.White);
            writer.Write($"{player.Mana} / {player.MaxMana}     ", Color.Blue);

            writer.CursorY += 2;
            writer.CursorX = 2;
            writer.WriteLine("Weapon:", Color.White);
            writer.CursorX = 2;
            if (player.Equipment.Weapon == null)
            {
                writer.WriteLine("[Nothing]", Color.Gray);
            }
            else
            {
                writer.WriteLine($"[{player.Equipment.Weapon.Name}]", ItemDrawingHelper.GetItemNameColor(player.Equipment.Weapon));
            }

            writer.CursorY++;

            writer.CursorX = 2;
            writer.WriteLine($"Damage: {player.Equipment.MinDamage} - {player.Equipment.MaxDamage}", Color.White);
            writer.CursorX = 2;
            writer.WriteLine($"Protection: {player.Equipment.Protection}", Color.White);
        }
    }
}