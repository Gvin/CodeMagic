using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Views
{
    public class LevelUpView : BaseWindow, ILevelUpView
    {
        private readonly int maxStatNameLength;
        private readonly FramedButton okButton;
        private readonly PlayerStats[] stats;

        public LevelUpView() : base(FontTarget.Interface)
        {
            StatsValue = new Dictionary<PlayerStats, int>();
            stats = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().ToArray();
            maxStatNameLength = stats.Select(TextHelper.GetStatName).Select(name => name.Length).Max() + 1;

            okButton = new FramedButton(new Rectangle(3, Height - 4, 20, 3))
            {
                Text = "OK"
            };
            okButton.Click += (sender, args) => Ok?.Invoke(this, EventArgs.Empty);
            Controls.Add(okButton);

            DoForAllStats((y, stat) =>
            {
                AddStatSelector(6, y, stat);
            });
        }

        private void DoForAllStats(Action<int, PlayerStats> action)
        {
            const int dY = 6;
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];
                var y = dY + index * 4;

                action(y, stat);
            }
        }

        private void PrintStatStatus(int x, int y, PlayerStats stat, ICellSurface surface)
        {
            int symbol = ' ';
            if (SelectedStat.HasValue && SelectedStat.Value == stat)
            {
                symbol = '▲';

            }
            surface.SetCell(x + maxStatNameLength + 5, y, new Cell(symbol, TextHelper.PositiveValueColor.ToXna()));
        }

        private void AddStatSelector(int x, int y, PlayerStats stat)
        {
            var button = new FramedButton(new Rectangle(x + maxStatNameLength + 7, y - 1, 3, 3))
            {
                Text = "+"
            };
            button.Click += (sender, args) =>
            {
                SelectedStat = stat;
                StatSelected?.Invoke(this, EventArgs.Empty);
            };
            Controls.Add(button);
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            const int dX = 5;

            surface.Write(dX, 2, $"You has reached level {Level}!");
            surface.Write(dX, 3, "Please select stat you want to increase:");

            DoForAllStats((y, stat) =>
            {
                var name = TextHelper.GetStatName(stat);
                name = $"{name}:";
                while (name.Length < maxStatNameLength)
                {
                    name += " ";
                }

                surface.Write(dX, y, name);
                surface.Write(dX + maxStatNameLength + 2, y, StatsValue[stat].ToString());
            });

            const int dY = 6;
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];
                var y = dY + index * 4;
                PrintStatStatus(5, y, stat, surface);
            }
        }

        public event EventHandler Ok;
        public event EventHandler StatSelected;

        public int Level { get; set; }

        public Dictionary<PlayerStats, int> StatsValue { get; }

        public PlayerStats? SelectedStat { get; set; }

        public bool OkButtonEnabled
        {
            get => okButton.Enabled;
            set => okButton.Enabled = value;
        }
    }
}