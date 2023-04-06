using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;
using Microsoft.Extensions.Logging;

namespace CodeMagic.UI.Presenters
{
    public interface ICheatsView : IView
    {
        event EventHandler Exit;
        event EventHandler CheatLevelUp;
        event EventHandler CheatHeal;
        event EventHandler CheatRestoreMana;
        event EventHandler CheatRestoreStamina;
        event EventHandler CheatRestoreStats;
    }

    public class CheatsPresenter : IPresenter
    {
        private readonly ILogger<CheatsPresenter> _logger;
        private readonly ICheatsView _view;

        public CheatsPresenter(ICheatsView view, ILogger<CheatsPresenter> logger)
        {
            _view = view;
            _logger = logger;

            _view.Exit += View_Exit;
            _view.CheatLevelUp += View_CheatLevelUp;
            _view.CheatHeal += View_CheatHeal;
            _view.CheatRestoreMana += View_CheatRestoreMana;
            _view.CheatRestoreStamina += View_CheatRestoreStamina;
            _view.CheatRestoreStats += View_CheatRestoreStats;
        }

        private void View_CheatRestoreStats(object sender, EventArgs e)
        {
            _logger.LogInformation("Used Cheat \"Restore Stats\"");
            _view.Close();
            CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
            CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
            ((Player)CurrentGame.Player).Stamina = ((Player)CurrentGame.Player).MaxStamina;
        }

        private void View_CheatRestoreStamina(object sender, EventArgs e)
        {
            _logger.LogInformation("Used Cheat \"Restore Stamina\"");
            _view.Close();
            ((Player)CurrentGame.Player).Stamina = ((Player)CurrentGame.Player).MaxStamina;
        }

        private void View_CheatRestoreMana(object sender, EventArgs e)
        {
            _logger.LogInformation("Used Cheat \"Restore Mana\"");
            _view.Close();
            CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
        }

        private void View_CheatHeal(object sender, EventArgs e)
        {
            _logger.LogInformation("Used Cheat \"Heal\"");
            _view.Close();
            CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
        }

        private void View_CheatLevelUp(object sender, EventArgs e)
        {
            _logger.LogInformation("Used Cheat \"Level Up\"");
            _view.Close();
            var exp = ((Player)CurrentGame.Player).GetXpToLevelUp() - CurrentGame.Player.Experience;
            CurrentGame.Player.AddExperience(exp);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            _view.Close();
        }

        public void Run()
        {
            _view.Show();
        }
    }
}