using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Controls;
using SadConsole;
using SadConsole.Input;
using ILog = CodeMagic.UI.Sad.Common.ILog;
using Point = SadRogue.Primitives.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class CheatsView : GameViewBase
    {
        public CheatsView()
            : base((ILog) LogManager.GetLog<CheatsView>())
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            Surface.Print(2, 1, "Cheats");

            var closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close",
            };
            closeButton.Click += closeButton_Click;
            ControlHostComponent.Add(closeButton);

            var levelUp = new StandardButton(20)
            {
                Text = "Level Up",
                Position = new Point(2, 3)
            };
            levelUp.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Level Up\"");

                Hide();
                var exp = ((Player) CurrentGame.Player).GetXpToLevelUp() - CurrentGame.Player.Experience;
                CurrentGame.Player.AddExperience(exp);
            };
            ControlHostComponent.Add(levelUp);

            var heal = new StandardButton(20)
            {
                Text = "Heal",
                Position = new Point(2, 7)
            };
            heal.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Heal\"");
                Hide();
                CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
            };
            ControlHostComponent.Add(heal);

            var restoreMana = new StandardButton(20)
            {
                Text = "Restore Mana",
                Position = new Point(2, 11)
            };
            restoreMana.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Restore Mana\"");
                Hide();
                CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
            };
            ControlHostComponent.Add(restoreMana);

            var restoreStamina = new StandardButton(20)
            {
                Text = "Restore Stamina",
                Position = new Point(2, 15)
            };
            restoreStamina.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Restore Stamina\"");
                Hide();
                ((Player)CurrentGame.Player).Stamina = ((Player)CurrentGame.Player).MaxStamina;
            };
            ControlHostComponent.Add(restoreStamina);

            var restoreStats = new StandardButton(20)
            {
                Text = "Restore Stats",
                Position = new Point(2, 19)
            };
            restoreStats.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Restore Stats\"");
                Hide();
                CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
                CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
                ((Player) CurrentGame.Player).Stamina = ((Player) CurrentGame.Player).MaxStamina;
            };
            ControlHostComponent.Add(restoreStats);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Hide();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}