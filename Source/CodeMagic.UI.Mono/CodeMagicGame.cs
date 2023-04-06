using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.UI.Mono.Extension;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Mono
{
    internal class CodeMagicGame : MonoConsoleGame
    {
        private readonly IGameManager _gameManager;
        private readonly IApplicationController _applicationController;
        private readonly ISaveService _saveService;

        public CodeMagicGame(IGameManager gameManager, IApplicationController applicationController, ISaveService saveService) 
            : base(FontProvider.GetScreenWidthPx(), FontProvider.GetScreenHeightPx())
        {
            _gameManager = gameManager;
            _applicationController = applicationController;
            _saveService = saveService;
            FontProvider.Initialize(GraphicsDevice);
        }

        protected override void BeginRun()
        {
            base.BeginRun();

            _gameManager.LoadGame();

            _applicationController.CreatePresenter<MainMenuPresenter>().Run();
        }

        protected override void EndRun()
        {
            base.EndRun();

            if (CurrentGame.Game != null)
            {
                _saveService.SaveGame(CurrentGame.Game, GameData.Current);
            }
        }
    }
}