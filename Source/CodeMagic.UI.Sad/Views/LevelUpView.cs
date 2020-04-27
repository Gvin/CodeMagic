using System;
using System.Linq;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace CodeMagic.UI.Sad.Views
{
    public class LevelUpView : View
    {
        private readonly Player player;
        private PlayerStats? selectedStat;

        private readonly int maxStatNameLength;
        private readonly PlayerStats[] stats;
        private StandardButton okButton;

        public LevelUpView(Player player)
        {
            this.player = player;
            selectedStat = null;

            stats = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().ToArray();
            maxStatNameLength = stats.Select(TextHelper.GetStatName).Select(name => name.Length).Max() + 1;

            PrintLevelUpText();
            InitializeControls();
        }

        private void InitializeControls()
        {
            okButton = new StandardButton(20)
            {
                Position = new Point(3, Height - 4),
                Text = "OK"
            };
            okButton.Click += (sender, args) =>
            {
                if (selectedStat.HasValue)
                {
                    player.IncreaseStat(selectedStat.Value);
                    Close();
                }
            };
            Add(okButton);
        }

        private void PrintLevelUpText()
        {
            const int dX = 5;
            
            Print(5, 2, $"You has reached level {player.Level}!");
            Print(5, 3, "Please select stat you want to increase:");

            const int dY = 6;
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];
                var y = dY + index * 4;
                AddStatSelector(dX, y, stat, maxStatNameLength);
            }
        }

        private void PrintStatStatus(int x, int y, PlayerStats stat)
        {
            int symbol = ' ';
            if (selectedStat.HasValue && selectedStat.Value == stat)
            {
                symbol = Glyphs.GetGlyph('▲');
                
            }
            Print(x + maxStatNameLength + 5, y, new ColoredGlyph(symbol, TextHelper.PositiveValueColor.ToXna(), DefaultBackground));
        }

        private void AddStatSelector(int x, int y, PlayerStats stat, int nameWidth)
        {
            var name = TextHelper.GetStatName(stat);
            name = $"{name}:";
            while (name.Length < nameWidth)
            {
                name += " ";
            }

            Print(x, y, name);
            Print(x + nameWidth + 2, y, player.GetPureStat(stat).ToString());

            var button = new StandardButton(3)
            {
                Text = "+",
                Position = new Point(x + nameWidth + 7, y - 1)
            };
            button.Click += (sender, args) =>
            {
                selectedStat = stat;
            };
            Add(button);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            okButton.IsEnabled = selectedStat.HasValue;
        }

        public override void Draw(TimeSpan update)
        {
            base.Draw(update);

            const int dY = 6;
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];
                var y = dY + index * 4;
                PrintStatStatus(5, y, stat);
            }
        }
    }
}