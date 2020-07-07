using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Game.Objects.Creatures;

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
        private static readonly ILog Log = LogManager.GetLog<CheatsPresenter>();

        private readonly ICheatsView view;

        public CheatsPresenter(ICheatsView view)
        {
            this.view = view;
            this.view.Exit += View_Exit;
            this.view.CheatLevelUp += View_CheatLevelUp;
            this.view.CheatHeal += View_CheatHeal;
            this.view.CheatRestoreMana += View_CheatRestoreMana;
            this.view.CheatRestoreStamina += View_CheatRestoreStamina;
            this.view.CheatRestoreStats += View_CheatRestoreStats;
        }

        private void View_CheatRestoreStats(object sender, EventArgs e)
        {
            Log.Info("Used Cheat \"Restore Stats\"");
            view.Close();
            CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
            CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
            ((Player)CurrentGame.Player).Stamina = ((Player)CurrentGame.Player).MaxStamina;
        }

        private void View_CheatRestoreStamina(object sender, EventArgs e)
        {
            Log.Info("Used Cheat \"Restore Stamina\"");
            view.Close();
            ((Player)CurrentGame.Player).Stamina = ((Player)CurrentGame.Player).MaxStamina;
        }

        private void View_CheatRestoreMana(object sender, EventArgs e)
        {
            Log.Info("Used Cheat \"Restore Mana\"");
            view.Close();
            CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
        }

        private void View_CheatHeal(object sender, EventArgs e)
        {
            Log.Info("Used Cheat \"Heal\"");
            view.Close();
            CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
        }

        private void View_CheatLevelUp(object sender, EventArgs e)
        {
            Log.Info("Used Cheat \"Level Up\"");
            view.Close();
            var exp = ((Player)CurrentGame.Player).GetXpToLevelUp() - CurrentGame.Player.Experience;
            CurrentGame.Player.AddExperience(exp);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            view.Close();
        }

        public void Run()
        {
            view.Show();
        }
    }
}