using CodeMagic.UI.Services;

namespace CodeMagic.UI.Mono.GameProcess
{
    public class ApplicationService : IApplicationService
    {
        public void Exit()
        {
            Program.Exit();
        }
    }
}