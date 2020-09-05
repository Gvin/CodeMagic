using CodeMagic.Core.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.UI.Mono.Extension;
using CodeMagic.UI.Mono.Fonts;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Mono
{
    internal class CodeMagicGame : MonoConsoleGame
    {
        public CodeMagicGame() 
            : base(FontProvider.GetScreenWidthPx(), FontProvider.GetScreenHeightPx())
        {
            FontProvider.Initialize(GraphicsDevice);
        }

        protected override void BeginRun()
        {
            base.BeginRun();

            IoC.Container.Resolve<IGameManager>().LoadGame();

            IoC.Container.Resolve<IApplicationController>().CreatePresenter<MainMenuPresenter>().Run();
        }

        protected override void EndRun()
        {
            base.EndRun();

            if (CurrentGame.Game != null)
            {
                IoC.Container.Resolve<ISaveService>().SaveGame();
            }
        }
    }
}