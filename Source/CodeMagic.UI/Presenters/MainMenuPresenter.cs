using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.UI.Presenters
{
    public interface IMainMenuView : IView
    {
        event EventHandler StartGame;

        event EventHandler ContinueGame;

        event EventHandler ShowSpellLibrary;

        event EventHandler ShowSettings;

        event EventHandler Exit;

        void SetContinueOptionState(bool canContinue);
    }

    public class MainMenuPresenter : IPresenter
    {
        private readonly IMainMenuView view;
        private readonly IApplicationController controller;
        private readonly IApplicationService applicationService;
        private readonly IGameManager gameManager;

        public MainMenuPresenter(
            IMainMenuView view, 
            IApplicationController controller, 
            IApplicationService applicationService, 
            IGameManager gameManager)
        {
            this.view = view;
            this.controller = controller;
            this.applicationService = applicationService;
            this.gameManager = gameManager;

            this.view.SetContinueOptionState(CurrentGame.Game != null);

            this.view.StartGame += View_StartGame;
            this.view.ContinueGame += View_ContinueGame;
            this.view.ShowSpellLibrary += View_ShowSpellLibrary;
            this.view.ShowSettings += View_ShowSettings;
            this.view.Exit += View_Exit;
        }

        private void View_Exit(object sender, EventArgs e)
        {
            applicationService.Exit();
        }

        private void View_ShowSettings(object sender, EventArgs e)
        {
            controller.CreatePresenter<SettingsPresenter>().Run();
        }

        private void View_ShowSpellLibrary(object sender, EventArgs e)
        {
            controller.CreatePresenter<MainSpellsLibraryPresenter>().Run();
        }

        private void View_ContinueGame(object sender, EventArgs e)
        {
            if (CurrentGame.Game == null)
                return;

            view.Close();

            var game = (GameCore<Player>)CurrentGame.Game;
            controller.CreatePresenter<GameViewPresenter>().Run(game);
        }

        private void View_StartGame(object sender, EventArgs args)
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