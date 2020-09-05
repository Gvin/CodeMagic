using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface IInGameMenuView : IView
    {
        event EventHandler ContinueGame;
        event EventHandler StartNewGame;
        event EventHandler Exit;
        event EventHandler ExitToMenu;
    }

    public class InGameMenuPresenter : IPresenter
    {
        private readonly IInGameMenuView view;
        private readonly IApplicationController controller;
        private readonly IGameManager gameManager;
        private readonly IApplicationService applicationService;
        private GameCore<Player> currentGame;

        public InGameMenuPresenter(
            IInGameMenuView view, 
            IApplicationController controller, 
            IGameManager gameManager,
            IApplicationService applicationService)
        {
            this.view = view;
            this.controller = controller;
            this.gameManager = gameManager;
            this.applicationService = applicationService;

            this.view.Exit += View_Exit;
            this.view.ExitToMenu += View_ExitToMenu;
            this.view.StartNewGame += View_StartNewGame;
            this.view.ContinueGame += View_ContinueGame;
        }

        private void View_ContinueGame(object sender, EventArgs e)
        {
            view.Close();

            controller.CreatePresenter<GameViewPresenter>().Run(currentGame);
        }

        private void View_StartNewGame(object sender, EventArgs e)
        {
            currentGame.Dispose();

            view.Close();

            var waitMessagePresenter = controller.CreatePresenter<WaitMessagePresenter>();
            waitMessagePresenter.Run("Starting new game...", () =>
            {
                var game = gameManager.StartGame();
                controller.CreatePresenter<GameViewPresenter>().Run(game);
            });
        }

        private void View_ExitToMenu(object sender, EventArgs e)
        {
            view.Close();

            controller.CreatePresenter<MainMenuPresenter>().Run();
        }

        private void View_Exit(object sender, EventArgs e)
        {
            applicationService.Exit();
        }

        public void Run(GameCore<Player> game)
        {
            currentGame = game;

            view.Show();
        }
    }
}