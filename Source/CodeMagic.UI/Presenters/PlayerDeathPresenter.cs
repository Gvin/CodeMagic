using System;
using CodeMagic.Game.GameProcess;

namespace CodeMagic.UI.Presenters
{
    public interface IPlayerDeathView : IView
    {
        event EventHandler StartNewGame;
        event EventHandler ExitToMenu;
        event EventHandler Exit;
    }

    public class PlayerDeathPresenter : IPresenter
    {
        private readonly IPlayerDeathView view;
        private readonly IApplicationController controller;
        private readonly IApplicationService applicationService;
        private readonly IGameManager gameManager;

        public PlayerDeathPresenter(
            IPlayerDeathView view, 
            IApplicationController controller, 
            IApplicationService applicationService, 
            IGameManager gameManager)
        {
            this.view = view;
            this.controller = controller;
            this.applicationService = applicationService;
            this.gameManager = gameManager;

            this.view.StartNewGame += View_StartNewGame;
            this.view.ExitToMenu += View_ExitToMenu;
            this.view.Exit += View_Exit;
        }

        private void View_Exit(object sender, EventArgs e)
        {
            applicationService.Exit();
        }

        private void View_ExitToMenu(object sender, EventArgs e)
        {
            view.Close();

            controller.CreatePresenter<MainMenuPresenter>().Run();
        }

        private void View_StartNewGame(object sender, EventArgs e)
        {
            view.Close();
            
            controller.CreatePresenter<WaitMessagePresenter>().Run("Starting new game...", () =>
            {
                var game = gameManager.StartGame();
                controller.CreatePresenter<GameViewPresenter>().Run(game);
            });
        }

        public void Run()
        {
            view.Show();
        }
    }
}