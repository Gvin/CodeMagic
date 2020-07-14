using System;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class CheatsView : BaseWindow, ICheatsView
    {
        public CheatsView() : base(FontTarget.Interface)
        {
            Controls.Add(new Label(2, 1)
            {
                Text = "Cheats"
            });

            var closeButton = new FramedButton(new Rectangle(Width - 17, Height - 4, 15, 3))
            {
                Text = "[ESC] Close"
            };
            closeButton.Click += (sender, args) => Exit?.Invoke(this, EventArgs.Empty);
            Controls.Add(closeButton);

            var levelUp = new FramedButton(new Rectangle(2, 3, 20, 3))
            {
                Text = "Level Up"
            };
            levelUp.Click += (sender, args) => CheatLevelUp?.Invoke(this, EventArgs.Empty);
            Controls.Add(levelUp);

            var heal = new FramedButton(new Rectangle(2, 7, 20, 3))
            {
                Text = "Heal"
            };
            heal.Click += (sender, args) => CheatHeal?.Invoke(this, EventArgs.Empty);
            Controls.Add(heal);

            var restoreMana = new FramedButton(new Rectangle(2, 11, 20, 3))
            {
                Text = "Restore Mana"
            };
            restoreMana.Click += (sender, args) => CheatRestoreMana?.Invoke(this, EventArgs.Empty);
            Controls.Add(restoreMana);

            var restoreStamina = new FramedButton(new Rectangle(2, 15, 20, 3))
            {
                Text = "Restore Stamina"
            };
            restoreStamina.Click += (sender, args) => CheatRestoreStamina?.Invoke(this, EventArgs.Empty);
            Controls.Add(restoreStamina);

            var restoreStats = new FramedButton(new Rectangle(2, 19, 20, 3))
            {
                Text = "Restore Stats"
            };
            restoreStats.Click += (sender, args) => CheatRestoreStats?.Invoke(this, EventArgs.Empty);
            Controls.Add(restoreStats);
        }

        protected override void ProcessKeyPressed(Keys key)
        {
            base.ProcessKeyPressed(key);

            switch (key)
            {
                case Keys.Escape:
                    Exit?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        public event EventHandler Exit;
        public event EventHandler CheatLevelUp;
        public event EventHandler CheatHeal;
        public event EventHandler CheatRestoreMana;
        public event EventHandler CheatRestoreStamina;
        public event EventHandler CheatRestoreStats;
    }
}