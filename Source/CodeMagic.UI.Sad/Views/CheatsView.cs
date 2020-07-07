using System;
using CodeMagic.Core.Logging;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using SadConsole.Input;
using ILog = CodeMagic.UI.Sad.Common.ILog;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class CheatsView : GameViewBase, ICheatsView
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
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Add(closeButton);

            var levelUp = new StandardButton(20)
            {
                Text = "Level Up",
                Position = new Point(2, 3)
            };
            levelUp.Click += (sender, args) => CheatLevelUp?.Invoke(this, EventArgs.Empty);
            Add(levelUp);

            var heal = new StandardButton(20)
            {
                Text = "Heal",
                Position = new Point(2, 7)
            };
            heal.Click += (sender, args) => CheatHeal?.Invoke(this, EventArgs.Empty);
            Add(heal);

            var restoreMana = new StandardButton(20)
            {
                Text = "Restore Mana",
                Position = new Point(2, 11)
            };
            restoreMana.Click += (sender, args) => CheatRestoreMana?.Invoke(this, EventArgs.Empty);
            Add(restoreMana);

            var restoreStamina = new StandardButton(20)
            {
                Text = "Restore Stamina",
                Position = new Point(2, 15)
            };
            restoreStamina.Click += (sender, args) => CheatRestoreStamina?.Invoke(this, EventArgs.Empty);
            Add(restoreStamina);

            var restoreStats = new StandardButton(20)
            {
                Text = "Restore Stats",
                Position = new Point(2, 19)
            };
            restoreStats.Click += (sender, args) => CheatRestoreStats?.Invoke(this, EventArgs.Empty);
            Add(restoreStats);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler Exit;
        public event EventHandler CheatLevelUp;
        public event EventHandler CheatHeal;
        public event EventHandler CheatRestoreMana;
        public event EventHandler CheatRestoreStamina;
        public event EventHandler CheatRestoreStats;
    }
}