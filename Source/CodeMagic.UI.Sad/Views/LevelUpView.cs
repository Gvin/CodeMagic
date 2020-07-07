using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad.Views
{
    public class LevelUpView : GameViewBase, ILevelUpView
    {
        private readonly int maxStatNameLength;
        private StandardButton okButton;
        private readonly PlayerStats[] stats;

        public LevelUpView()
        {
            StatsValue = new Dictionary<PlayerStats, int>();
            stats = Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>().ToArray();
            maxStatNameLength = stats.Select(TextHelper.GetStatName).Select(name => name.Length).Max() + 1;
            
            InitializeControls();
        }

        private void InitializeControls()
        {
            okButton = new StandardButton(20)
            {
                Position = new Point(3, Height - 4),
                Text = "OK"
            };
            okButton.Click += (sender, args) => Ok?.Invoke(this, EventArgs.Empty);
            Add(okButton);

            DoForAllStats((y, stat) =>
            {
                AddStatSelector(6, y, stat);
            });
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            const int dX = 5;

            surface.Print(dX, 2, $"You has reached level {Level}!");
            surface.Print(dX, 3, "Please select stat you want to increase:");

            DoForAllStats((y, stat) =>
            {
                var name = TextHelper.GetStatName(stat);
                name = $"{name}:";
                while (name.Length < maxStatNameLength)
                {
                    name += " ";
                }

                surface.Print(dX, y, name);
                surface.Print(dX + maxStatNameLength + 2, y, StatsValue[stat].ToString());
            });

            const int dY = 6;
            for (var index = 0; index < stats.Length; index++)
            {
                var stat = stats[index];
                var y = dY + index * 4;
                PrintStatStatus(5, y, stat, surface);
            }
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

        private void PrintStatStatus(int x, int y, PlayerStats stat, CellSurface surface)
        {
            int symbol = ' ';
            if (SelectedStat.HasValue && SelectedStat.Value == stat)
            {
                symbol = Glyphs.GetGlyph('▲');
                
            }
            surface.Print(x + maxStatNameLength + 5, y, new ColoredGlyph(symbol, TextHelper.PositiveValueColor.ToXna(), DefaultBackground));
        }

        private void AddStatSelector(int x, int y, PlayerStats stat)
        {
            var button = new StandardButton(3)
            {
                Text = "+",
                Position = new Point(x + maxStatNameLength + 7, y - 1)
            };
            button.Click += (sender, args) =>
            {
                SelectedStat = stat;
                StatSelected?.Invoke(this, EventArgs.Empty);
            };
            Add(button);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler Ok;
        public event EventHandler StatSelected;
        public int Level { get; set; }
        public Dictionary<PlayerStats, int> StatsValue { get; }
        public PlayerStats? SelectedStat { get; set; }

        public bool OkButtonEnabled
        {
            get => okButton.IsEnabled;
            set => okButton.IsEnabled = value;
        }
    }
}