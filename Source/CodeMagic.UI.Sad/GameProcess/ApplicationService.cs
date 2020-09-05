using CodeMagic.UI.Services;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class ApplicationService : IApplicationService
    {
        public void Exit()
        {
            Program.Exit();
        }
    }
}