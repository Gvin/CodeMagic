using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Controls.VisualControls;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Orientation = SadConsole.Orientation;
using Point = Microsoft.Xna.Framework.Point;
using ScrollBar = SadConsole.Controls.ScrollBar;

namespace CodeMagic.UI.Sad.Views
{
    public class GameView : GameViewBase, IGameView
    {
        private static readonly TimeSpan KeyProcessFrequency = TimeSpan.FromMilliseconds(Settings.Current.MinActionsInterval);

        private GameAreaControl gameArea;

        private ScrollBar journalScroll;
        private JournalBoxControl journalBox;

        private StandardButton openSpellBookButton;
        private StandardButton openInventoryButton;
        private StandardButton showItemsOnFloorButton;
        private StandardButton openPlayerStatsButton;

        private DateTime lastKeyProcessed;

        public GameView() 
            : base(FontTarget.Game)
        {
            lastKeyProcessed = DateTime.Now;

            UseKeyboard = true;
        }

        public void Initialize()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
#if DEBUG
            var cheatsButton = new StandardButton(3)
            {
                Text = "*",
                Position = new Point(Width - 39, 37)
            };
            cheatsButton.Click += (sender, args) => OpenCheats?.Invoke(this, EventArgs.Empty);
            Add(cheatsButton);
#endif

            var playerStatsControl = new PlayerStatsVisualControl(
                new Rectangle(Width - 40, 1, 39, 65), 
                Game);
            AddVisualControl(playerStatsControl);

            gameArea = new GameAreaControl(Game)
            {
                Position = new Point(1, 1)
            };
            Add(gameArea);

            journalScroll = new ScrollBar(Orientation.Vertical, 9)
            {
                Position = new Point(1, Height - 10),
                CanFocus = false,
                Theme = new ScrollBarTheme
                {
                    Normal = new Cell(Color.White, Color.Black)
                }
            };
            Add(journalScroll);

            journalBox = new JournalBoxControl(Width - 3, 10, journalScroll, Game.Journal)
            {
                Position = new Point(2, Height - 11)
            };
            Add(journalBox);


            openInventoryButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 22),
                Text = "[I] Inventory"
            };
            openInventoryButton.Click += (sender, args) => OpenInventory?.Invoke(this, EventArgs.Empty);
            Add(openInventoryButton);

            openSpellBookButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 25),
                Text = "[C] Spell Book"
            };
            openSpellBookButton.Click += (sender, args) => OpenSpellBook?.Invoke(this, EventArgs.Empty);
            openSpellBookButton.IsEnabled = true;
            Add(openSpellBookButton);

            showItemsOnFloorButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 28),
                Text = "[G] Check Floor"
            };
            showItemsOnFloorButton.Click += (sender, args) => OpenGroundView?.Invoke(this, EventArgs.Empty);
            Add(showItemsOnFloorButton);

            openPlayerStatsButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 31),
                Text = "[V] Player Status"
            };
            openPlayerStatsButton.Click += (sender, args) => OpenPlayerStats?.Invoke(this, EventArgs.Empty);
            Add(openPlayerStatsButton);
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
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
                case Keys.Escape:
                    OpenInGameMenu?.Invoke(this, EventArgs.Empty);
                    return true;
            }

            if (PerformKeyPlayerAction(key.Key))
                return true;

            return base.ProcessKeyPressed(key);
        }

        protected override bool ProcessKeyDown(AsciiKey key)
        {
            if (PerformKeyPlayerAction(key.Key))
            {
                return true;
            }

            return base.ProcessKeyDown(key);
        }

        private bool PerformKeyPlayerAction(Keys key)
        {
            if (DateTime.Now - lastKeyProcessed < KeyProcessFrequency)
                return false;

            var action = GetPlayerAction(key);
            if (action == null)
                return false;

            lastKeyProcessed = DateTime.Now;
            Game.PerformPlayerAction(action);
            return true;
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

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            // Journal box frame
            surface.Print(0, Height - 11, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            surface.Print(1, Height - 11, new ColoredGlyph(Glyphs.GetGlyph('─'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, Height - 11, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            // Player stats frame
            surface.Print(Width - 40, 0, new ColoredGlyph(Glyphs.GetGlyph('╤'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, 3, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

        }

        public void Close()
        {
            Close(DialogResult.None);
        }

        public event EventHandler OpenInGameMenu;
        public event EventHandler OpenSpellBook;
        public event EventHandler OpenInventory;
        public event EventHandler OpenPlayerStats;
        public event EventHandler OpenGroundView;
        public event EventHandler OpenCheats;

        public bool SpellBookEnabled
        {
            set => openSpellBookButton.IsEnabled = value;
        }

        public bool OpenGroundEnabled
        {
            set => showItemsOnFloorButton.IsEnabled = value;
        }

        public GameCore<Player> Game { get; set; }
    }
}