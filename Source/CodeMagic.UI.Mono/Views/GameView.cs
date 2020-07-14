using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Mono.ActivePlanes;
using CodeMagic.UI.Mono.Controls;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Mono.Views
{
    public class GameView : BaseWindow, IGameView
    {
        private static readonly TimeSpan KeyProcessFrequency = TimeSpan.FromMilliseconds(Settings.Current.MinActionsInterval);

        const int RightPanelWidth = 30;

        private ProgressBar healthBar;
        private ProgressBar manaBar;
        private ProgressBar staminaBar;
        private ProgressBar hungerBar;

        private readonly FramedButton openSpellBookButton;
        private readonly FramedButton showItemsOnFloorButton;

        private DateTime lastKeyProcessed;
        private int journalBoxY;

        public GameView() : base(FontTarget.Interface)
        {
            var menuButtonsPosition = Width - RightPanelWidth;
            var menuButtonsWidth = RightPanelWidth - 1;

#if DEBUG
            var cheatsButton = new FramedButton(new Rectangle(menuButtonsPosition, 37, 3, 3))
            {
                Text = "*",
            };
            cheatsButton.Click += (sender, args) => OpenCheats?.Invoke(this, EventArgs.Empty);
            Controls.Add(cheatsButton);
#endif

            var openInventoryButton = new FramedButton(new Rectangle(menuButtonsPosition, 22, menuButtonsWidth, 3))
            {
                Text = "[I] Inventory"
            };
            openInventoryButton.Click += (sender, args) => OpenInventory?.Invoke(this, EventArgs.Empty);
            Controls.Add(openInventoryButton);

            openSpellBookButton = new FramedButton(new Rectangle(menuButtonsPosition, 25, menuButtonsWidth, 3))
            {
                Text = "[C] Spell Book"
            };
            openSpellBookButton.Click += (sender, args) => OpenSpellBook?.Invoke(this, EventArgs.Empty);
            Controls.Add(openSpellBookButton);

            showItemsOnFloorButton = new FramedButton(new Rectangle(menuButtonsPosition, 28, menuButtonsWidth, 3))
            {
                Text = "[G] Check Floor"
            };
            showItemsOnFloorButton.Click += (sender, args) => OpenGroundView?.Invoke(this, EventArgs.Empty);
            Controls.Add(showItemsOnFloorButton);

            var openPlayerStatsButton = new FramedButton(new Rectangle(menuButtonsPosition, 31, menuButtonsWidth, 3))
            {
                Text = "[V] Player Status"
            };
            openPlayerStatsButton.Click += (sender, args) => OpenPlayerStats?.Invoke(this, EventArgs.Empty);
            Controls.Add(openPlayerStatsButton);
        }

        public event EventHandler OpenInGameMenu;
        public event EventHandler OpenSpellBook;
        public event EventHandler OpenInventory;
        public event EventHandler OpenPlayerStats;
        public event EventHandler OpenGroundView;
        public event EventHandler OpenCheats;

        public bool SpellBookEnabled
        {
            set => openSpellBookButton.Enabled = value;
        }

        public bool OpenGroundEnabled
        {
            set => showItemsOnFloorButton.Enabled = value;
        }

        public GameCore<Player> Game { private get; set; }

        public void Initialize()
        {
            var rightPanelX = Width - RightPanelWidth;

            healthBar = new ProgressBar(new Rectangle(rightPanelX, 4, RightPanelWidth - 1, 1))
            {
                MaxValue = Game.Player.MaxHealth,
                Value = Game.Player.Health,
                Theme = new ProgressBarTheme
                {
                    EmptyBar = new Cell('█', Color.FromNonPremultiplied(112, 0, 0, 255)),
                    FilledBar = new Cell('.', Color.Black, Color.Red)
                }
            };
            Controls.Add(healthBar);

            manaBar = new ProgressBar(new Rectangle(rightPanelX, 6, RightPanelWidth - 1, 1))
            {
                MaxValue = Game.Player.MaxMana,
                Value = Game.Player.Mana,
                Theme = new ProgressBarTheme
                {
                    EmptyBar = new Cell('█', Color.DarkBlue),
                    FilledBar = new Cell('.', Color.Black, Color.Blue)
                }
            };
            Controls.Add(manaBar);

            staminaBar = new ProgressBar(new Rectangle(rightPanelX, 8, RightPanelWidth - 1, 1))
            {
                MaxValue = Game.Player.MaxStamina,
                Value = Game.Player.Stamina,
                Theme = new ProgressBarTheme
                {
                    EmptyBar = new Cell('█', Color.FromNonPremultiplied(142, 102, 8, 255)),
                    FilledBar = new Cell('.', Color.Black, Color.Gold)
                }
            };
            Controls.Add(staminaBar);

            hungerBar = new ProgressBar(new Rectangle(rightPanelX, 10, RightPanelWidth - 1, 1))
            {
                MaxValue = 100,
                Value = Game.Player.HungerPercent,
                Theme = new ProgressBarTheme
                {
                    EmptyBar = new Cell('█', Color.FromNonPremultiplied(0, 102, 0, 255)),
                    FilledBar = new Cell('.', Color.Black, Color.Lime)
                }
            };
            Controls.Add(hungerBar);

            var gameArea = new GameAreaActivePlane(new Point(Font.GlyphWidth, Font.GlyphHeight), Game);
            ActivePlanes.Add(gameArea);

            journalBoxY = (int) Math.Ceiling(gameArea.PixelHeight / (double) Font.GlyphHeight) + 2;
            var journalBoxHeight = Height - journalBoxY - 1;
            var journalScroll = new VerticalScrollBar(new Point(1, journalBoxY), journalBoxHeight);
            Controls.Add(journalScroll);
            Controls.Add(new JournalBoxControl(
                new Rectangle(1, journalBoxY, Width - 2, journalBoxHeight),
                Game.Journal, 
                journalScroll));
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            var rightPanelX = Width - RightPanelWidth;

            DrawJournalBoxFrame(surface);
            DrawPlayerStatus(surface, rightPanelX);
        }

        private void DrawJournalBoxFrame(ICellSurface surface)
        {
            surface.Fill(new Rectangle(1, journalBoxY - 1, Width - 2, 1), new Cell('─', FrameColor));
            surface.SetCell(0, journalBoxY - 1, new Cell('╟', FrameColor));
            surface.SetCell(Width - 1, journalBoxY - 1, new Cell('╢', FrameColor));
        }

        private void DrawPlayerStatus(ICellSurface surface, int rightPanelX)
        {
            surface.Write(rightPanelX, 1, "Player Status");
            surface.SetCell(rightPanelX - 1, 0, new Cell('╤', FrameColor));
            surface.Fill(new Rectangle(rightPanelX, 2, RightPanelWidth - 1, 1), new Cell('─', FrameColor));
            surface.SetCell(Width - 1, 2, new Cell('╢', FrameColor));
            surface.Fill(new Rectangle(rightPanelX - 1, 1, 1, journalBoxY - 1), new Cell('│', FrameColor));
            surface.SetCell(rightPanelX - 1, journalBoxY - 1, new Cell('┴', FrameColor));
            surface.SetCell(rightPanelX - 1, 2, new Cell('├', FrameColor));

            surface.Write(rightPanelX, 3, $"Health [{Game.Player.Health} / {Game.Player.MaxHealth}]");

            surface.Write(rightPanelX, 5, $"Mana [{Game.Player.Mana} / {Game.Player.MaxMana}]");

            surface.Write(rightPanelX, 7, $"Stamina [{Game.Player.Stamina} / {Game.Player.MaxStamina}]");

            surface.Write(rightPanelX, 9, $"Hunger [{Game.Player.HungerPercent}%]");

            surface.Write(rightPanelX, 11, "Area Mana");
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            healthBar.MaxValue = Game.Player.MaxHealth;
            healthBar.Value = Game.Player.Health;

            manaBar.MaxValue = Game.Player.MaxMana;
            manaBar.Value = Game.Player.Mana;

            staminaBar.MaxValue = Game.Player.MaxStamina;
            staminaBar.Value = Game.Player.Stamina;

            hungerBar.Value = Game.Player.HungerPercent;

            var keyboard = Keyboard.GetState();
            PerformKeyPlayerAction(keyboard);
        }

        public override bool ProcessKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Escape:
                    OpenInGameMenu?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.C:
                    OpenSpellBook?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.I:
                    OpenInventory?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.G:
                    OpenGroundView?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.V:
                    OpenPlayerStats?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            return base.ProcessKeyPressed(key);
        }

        private void PerformKeyPlayerAction(KeyboardState keyboardState)
        {
            if (DateTime.Now - lastKeyProcessed < KeyProcessFrequency)
                return;

            var pressedKeys = keyboardState.GetPressedKeys();
            if (pressedKeys.Length == 0 || pressedKeys.Length > 1)
                return;

            var action = GetPlayerAction(pressedKeys[0]);
            if (action == null)
                return;

            lastKeyProcessed = DateTime.Now;
            Game.PerformPlayerAction(action);
        }

        private IPlayerAction GetPlayerAction(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                case Keys.Up:
                    return new MovePlayerAction(Direction.North);
                case Keys.S:
                case Keys.Down:
                    return new MovePlayerAction(Direction.South);
                case Keys.A:
                case Keys.Left:
                    return new MovePlayerAction(Direction.West);
                case Keys.D:
                case Keys.Right:
                    return new MovePlayerAction(Direction.East);
                case Keys.Space:
                    return new EmptyPlayerAction();
                case Keys.F:
                    return new MeleeAttackPlayerAction(true);
                case Keys.R:
                    return new MeleeAttackPlayerAction(false);
                case Keys.E:
                    return new UseObjectPlayerAction();
                default:
                    return null;
            }
        }
    }
}