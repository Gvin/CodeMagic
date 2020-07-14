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

            IoC.Container.Resolve<IApplicationController>().CreatePresenter<MainMenuPresenter>().Run();
        }

        protected override int KeyPressedDelay => 100;
    }
}