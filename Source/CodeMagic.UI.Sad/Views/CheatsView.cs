using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Controls;
using SadConsole.Input;
using ILog = CodeMagic.UI.Sad.Common.ILog;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Point = Microsoft.Xna.Framework.Point;

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
            Print(2, 1, "Cheats");

            var closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close",
            };
            closeButton.Click += closeButton_Click;
            Add(closeButton);

            var levelUp = new StandardButton(20)
            {
                Text = "Level Up",
                Position = new Point(2, 3)
            };
            levelUp.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Level Up\"");
                Close();
                var exp = ((Player) CurrentGame.Player).GetXpToLevelUp() - CurrentGame.Player.Experience;
                CurrentGame.Player.AddExperience(exp);
            };
            Add(levelUp);

            var heal = new StandardButton(20)
            {
                Text = "Heal",
                Position = new Point(2, 7)
            };
            heal.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Heal\"");
                Close();
                CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
            };
            Add(heal);

            var restoreMana = new StandardButton(20)
            {
                Text = "Restore Mana",
                Position = new Point(2, 11)
            };
            restoreMana.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Restore Mana\"");
                Close();
                CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
            };
            Add(restoreMana);

            var restoreStats = new StandardButton(20)
            {
                Text = "Restore Stats",
                Position = new Point(2, 15)
            };
            restoreStats.Click += (sender, args) =>
            {
                Log.Info("Used Cheat \"Restore Stats\"");
                Close();
                CurrentGame.Player.Health = CurrentGame.Player.MaxHealth;
                CurrentGame.Player.Mana = CurrentGame.Player.MaxMana;
            };
            Add(restoreStats);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Close();
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}