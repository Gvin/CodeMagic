using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class GameView : View
    {
        private readonly IGameCore game;

        private PlayerStatsControl playerStats;
        private GameAreaControl gameArea;

        private ScrollBar journalScroll;
        private JournalBoxControl journalBox;

        private Button openSpellBookButton;

        private ButtonTheme standardButtonTheme;
        private ButtonTheme disabledButtonTheme;

        public GameView(IGameCore game) 
            : base(Program.Width, Program.Height)
        {
            UseKeyboard = true;

            this.game = game;

            InitializeControls();

            game.Player.Died += Player_Died;
        }

        private void Player_Died(object sender, EventArgs e)
        {
            Close();

            new PlayerDeathView().Show();
        }

        private void InitializeControls()
        {
            playerStats = new PlayerStatsControl(40, 40, game.Player)
            {
                Position = new Point(80, 0)
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
                    Colors = new Colors
                    {
                        Appearance_ControlNormal = new Cell(Color.White, Color.Black)
                    }
                }
            };
            Add(journalScroll);

            journalBox = new JournalBoxControl(Width - 3, 10, journalScroll, game.Journal)
            {
                Position = new Point(2, Height - 11)
            };
            Add(journalBox);

            standardButtonTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.White, DefaultBackground),
                    Appearance_ControlDisabled = new Cell(Color.Gray, DefaultBackground)
                },
            };
            disabledButtonTheme = new ButtonLinesTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(Color.Gray, DefaultBackground),
                    Appearance_ControlDisabled = new Cell(Color.Gray, DefaultBackground)
                },
            };
            openSpellBookButton = new Button(20, 3)
            {
                Position = new Point(81, 20),
                Text = "[C] Spell Book",
                CanFocus = false
            };
            openSpellBookButton.Click += openSpellBookButton_Click;
            SetButtonEnabled(openSpellBookButton, true);
            Add(openSpellBookButton);
        }

        private void SetButtonEnabled(Button button, bool enabled)
        {
            button.IsEnabled = enabled;
            button.Theme = enabled ? standardButtonTheme : disabledButtonTheme;
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.C:
                    OpenSpellBook();
                    return true;
                case Keys.Escape:
                    OpenMainMenu();
                    return true;
            }

            var action = GetPlayerAction(key.Key);
            if (action == null)
                return base.ProcessKeyPressed(key);

            game.PerformPlayerAction(action);
            return true;
        }

        private IPlayerAction GetPlayerAction(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                case Keys.Up:
                    return new MovePlayerAction(Direction.Up);
                case Keys.S:
                case Keys.Down:
                    return new MovePlayerAction(Direction.Down);
                case Keys.A:
                case Keys.Left:
                    return new MovePlayerAction(Direction.Left);
                case Keys.D:
                case Keys.Right:
                    return new MovePlayerAction(Direction.Right);
                case Keys.Space:
                    return new EmptyPlayerAction();
                case Keys.F:
                    return new MeleAttackPlayerAction();
                default:
                    return null;
            }
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

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(0, Height - 11, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleRight, FrameColor, DefaultBackground));
            Print(1, Height - 11, new ColoredGlyph(Glyphs.GlyphBoxSingleHorizontal, FrameColor, DefaultBackground));
            Print(Width - 1, Height - 11, new ColoredGlyph(Glyphs.GlyphBoxDoubleVerticalSingleLeft, FrameColor, DefaultBackground));
        }
    }
}