using CodeMagic.UI.Services;

namespace CodeMagic.UI.Mono.GameProcess;

public class ApplicationService : IApplicationService
{
    private readonly ICodeMagicGame _game;

    public ApplicationService(ICodeMagicGame game)
    {
        _game = game;
    }

    public void Exit()
    {
        _game.Exit();
    }
}
