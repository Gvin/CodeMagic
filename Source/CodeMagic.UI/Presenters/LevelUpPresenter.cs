using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.UI.Presenters
{
    public interface ILevelUpView : IView
    {
        event EventHandler Ok;
        event EventHandler StatSelected;

        int Level { get; set; }

        Dictionary<PlayerStats, int> StatsValue { get; }

        PlayerStats? SelectedStat { get; set; }

        bool OkButtonEnabled { get; set; }
    }

    public class LevelUpPresenter : IPresenter
    {
        private readonly ILevelUpView view;
        private Player player;

        public LevelUpPresenter(ILevelUpView view)
        {
            this.view = view;

            this.view.SelectedStat = null;
            this.view.OkButtonEnabled = false;

            this.view.Ok += View_Ok;
            this.view.StatSelected += View_StatSelected;
        }

        private void View_StatSelected(object sender, EventArgs e)
        {
            view.OkButtonEnabled = true;
        }

        private void View_Ok(object sender, EventArgs e)
        {
            if (view.SelectedStat.HasValue)
            {
                player.IncreaseStat(view.SelectedStat.Value);
                view.Close();
            }
        }

        public void Run(Player currentPlayer)
        {
            player = currentPlayer;
            view.Level = player.Level;

            foreach (var playerStat in Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>())
            {
                view.StatsValue.Add(playerStat, player.GetPureStat(playerStat));
            }

            view.Show();
        }
    }
}