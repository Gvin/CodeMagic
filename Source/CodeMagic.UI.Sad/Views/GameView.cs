using System;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.PlayerActions;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
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
    public class GameView : View
    {
        private static readonly TimeSpan KeyProcessFrequency = TimeSpan.FromMilliseconds(Settings.Current.MinActionsInterval);

        private readonly GameCore<Player> game;

        private PlayerStatsControl playerStats;
        private GameAreaControl gameArea;

        private ScrollBar journalScroll;
        private JournalBoxControl journalBox;

        private StandardButton openSpellBookButton;
        private StandardButton openInventoryButton;
        private StandardButton showItemsOnFloorButton;
        private StandardButton openPlayerStatsButton;
        private StandardButton cheatsButton;

        private DateTime lastKeyProcessed;

        public GameView(GameCore<Player> game) 
            : base(FontTarget.Game)
        {
            lastKeyProcessed = DateTime.Now;

            UseKeyboard = true;

            this.game = game;

            InitializeControls();

            game.Player.Died += Player_Died;
            game.Player.LeveledUp += Player_LeveledUp;
        }

        private void Player_LeveledUp(object sender, EventArgs e)
        {
            new LevelUpView(game.Player).Show();
        }

        private void Player_Died(object sender, EventArgs e)
        {
            Close();

            new PlayerDeathView().Show();
        }

        private void InitializeControls()
        {
#if DEBUG
            cheatsButton = new StandardButton(3)
            {
                Text = "*",
                Position = new Point(Width - 39, 35)
            };
            cheatsButton.Click += (sender, args) => { new CheatsView().Show(); };
            Add(cheatsButton);
#endif

            playerStats = new PlayerStatsControl(40, 65, game)
            {
                Position = new Point(Width - 40, 0)
            };
            Add(playerStats);

            gameArea = new GameAreaControl(game)
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

            journalBox = new JournalBoxControl(Width - 3, 10, journalScroll, game.Journal)
            {
                Position = new Point(2, Height - 11)
            };
            Add(journalBox);


            openInventoryButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 16),
                Text = "[I] Inventory"
            };
            openInventoryButton.Click += openInventoryButton_Click;
            Add(openInventoryButton);

            openSpellBookButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 19),
                Text = "[C] Spell Book"
            };
            openSpellBookButton.Click += openSpellBookButton_Click;
            openSpellBookButton.IsEnabled = true;
            Add(openSpellBookButton);

            showItemsOnFloorButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 22),
                Text = "[G] Check Floor"
            };
            showItemsOnFloorButton.Click += showItemsOnFloorButton_Click;
            Add(showItemsOnFloorButton);

            openPlayerStatsButton = new StandardButton(30)
            {
                Position = new Point(Width - 39, 25),
                Text = "[V] Player Status"
            };
            openPlayerStatsButton.Click += (sender, args) => OpenPlayerStats();
            Add(openPlayerStatsButton);
        }

        private void showItemsOnFloorButton_Click(object sender, EventArgs e)
        {
            ShowItemsOnFloor();
        }

        private void ShowItemsOnFloor()
        {
            var cell = game.Map.GetCell(game.PlayerPosition);
            var itemsOnFloor = cell.Objects.OfType<IItem>().ToArray();
            if (itemsOnFloor.Length == 0)
                return;
            
            var floorInventory = new Inventory(itemsOnFloor);
            floorInventory.ItemRemoved += (sender, args) => game.Map.RemoveObject(game.PlayerPosition, args.Item);

            new CustomInventoryView(game, "Items On Floor", floorInventory).Show();
        }

        private void openInventoryButton_Click(object sender, EventArgs e)
        {
            OpenInventory();
        }

        private void OpenInventory()
        {
            new PlayerInventoryView(game).Show();
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.C:
                    OpenSpellBook();
                    return true;
                case Keys.I:
                    OpenInventory();
                    return true;
                case Keys.G:
                    ShowItemsOnFloor();
                    return true;
                case Keys.V:
                    OpenPlayerStats();
                    return true;
                case Keys.Escape:
                    OpenMainMenu();
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
            game.PerformPlayerAction(action);
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
                    return new MeleAttackPlayerAction(true);
                case Keys.R:
                    return new MeleAttackPlayerAction(false);
                case Keys.E:
                    return new UseObjectPlayerAction();
                default:
                    return null;
            }
        }

        private void OpenPlayerStats()
        {
            new PlayerStatsView(game.Player).Show();
        }

        private void OpenMainMenu()
        {
            Close();

            new InGameMenuView(game).Show();
        }

        private void openSpellBookButton_Click(object sender, EventArgs args)
        {
            OpenSpellBook();
        }

        private void OpenSpellBook()
        {
            if (game.Player.Equipment.SpellBook == null)
                return;

            var spellBookView = new SpellBookView(game);
            spellBookView.Show();
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(0, Height - 11, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            surface.Print(1, Height - 11, new ColoredGlyph(Glyphs.GetGlyph('─'), FrameColor, DefaultBackground));
            surface.Print(Width - 1, Height - 11, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            openSpellBookButton.IsEnabled = game.Player.Equipment.SpellBook != null;
            showItemsOnFloorButton.IsEnabled = game.Map.GetCell(game.PlayerPosition).Objects.OfType<IItem>().Any();
        }

        protected override void OnClosed(DialogResult result)
        {
            base.OnClosed(result);

            game.Player.Died -= Player_Died;
            game.Player.LeveledUp -= Player_LeveledUp;
        }
    }
}