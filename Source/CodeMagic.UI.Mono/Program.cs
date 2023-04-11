using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeMagic.UI.Mono
{
    internal class Program
    {
        public const int MapCellImageSize = 7;

        private static ILogger<Program> _logger;

        [STAThread]
        internal static async Task Main()
        {
            var services = new ServiceCollection();

            var startup = new Startup();
            startup.ConfigureServices(services);

            await using var provider = services.BuildServiceProvider();

            try
            {
                _logger = provider.GetRequiredService<ILogger<Program>>();

                _logger.LogInformation("Initializing game. Version {Version}", Assembly.GetExecutingAssembly().GetName().Version);

                await startup.Initialize(provider);

                _logger.LogInformation("Starting the game.");

                var game = provider.GetRequiredService<ICodeMagicGame>();
                game.Run();

                _logger.LogInformation("Closing the game.");
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Unexpected error.");

#if DEBUG
                throw;
#endif
            }
        }
    }
}
