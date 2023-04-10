using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Drawing.ImageProviding;
using CodeMagic.UI.Mono.Extension.Glyphs;
using CodeMagic.UI.Mono.GameProcess;
using CodeMagic.UI.Mono.Saving;
using CodeMagic.UI.Mono.Views;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CodeMagic.UI.Mono
{
    internal class Program
    {
        private const string LogFilePath = @".\log_.txt";
        private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;

        public const int MapCellImageSize = 7;

        private static CodeMagicGame _game;

        private static ILogger<Program> _logger;

        [STAThread]
        internal static async Task Main()
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.File(LogFilePath, GetLogLevel(),
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 5242880, retainedFileCountLimit: 5)
                    .CreateLogger());
            });

            // Views
            services.AddTransient<ICheatsView, CheatsView>();
            services.AddTransient<ICustomInventoryView, CustomInventoryView>();
            services.AddTransient<IEditSpellView, EditSpellView>();
            services.AddTransient<IGameView, GameView>();
            services.AddTransient<IInGameMenuView, InGameMenuView>();
            services.AddTransient<ILevelUpView, LevelUpView>();
            services.AddTransient<ILoadSpellView, LoadSpellView>();
            services.AddTransient<IMainMenuView, MainMenuView>();
            services.AddTransient<IMainSpellsLibraryView, MainSpellsLibraryView>();
            services.AddTransient<IPlayerDeathView, PlayerDeathView>();
            services.AddTransient<IPlayerInventoryView, PlayerInventoryView>();
            services.AddTransient<IPlayerStatsView, PlayerStatsView>();
            services.AddTransient<ISettingsView, SettingsView>();
            services.AddTransient<ISpellBookView, SpellBookView>();
            services.AddTransient<IWaitMessageView, WaitMessageView>();

            // Presenters
            foreach (var presenterType in typeof(MainMenuPresenter).Assembly.GetTypes().Where(type => 
                         type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IPresenter))))
            {
                if (services.Any(registration => registration.ServiceType == presenterType))
                {
                    continue;
                }

                services.AddTransient(presenterType);
            }

            // Services
            services.AddSingleton<IImagesStorage, ImagesStorage>();
            services.AddSingleton<IInventoryImagesFactory, InventoryImagesFactory>();
            services.AddSingleton<IWorldImagesFactory, WorldImagesFactory>();
            services.AddSingleton<ILightLevelManager>(_ => new LightLevelManager(Settings.Current.Brightness));
            services.AddSingleton<ICellImageService, CellImageService>();
            services.AddSingleton<IApplicationController, ApplicationController>();
            services.AddSingleton<IDialogsProvider, DialogsProvider>();
            services.AddTransient<IEditSpellService, EditSpellService>();
            services.AddSingleton<ISpellsLibraryService, SpellsLibraryService>();
            services.AddSingleton<ISettingsService>(_ => Settings.Current);
            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddSingleton<ISaveService, SaveService>();
            services.AddSingleton<IGameManager, GameManager>();
            services.Configure<BasicGameConfiguration>(config =>
                {
                    config.SavingInterval = Settings.Current.SavingInterval;
                });

            services.AddSingleton<CodeMagicGame>();

            await using var provider = services.BuildServiceProvider();

            try
            {
                _logger = provider.GetRequiredService<ILogger<Program>>();

                _logger.LogInformation($"Initializing game. Version {Assembly.GetExecutingAssembly().GetName().Version}");

                var imageStorage = provider.GetRequiredService<IImagesStorage>();
                await imageStorage.Load();

#if DEBUG
                PerformanceMeter.Initialize(@".\Performance.log");
#endif

                var config = GameConfigurator.LoadConfiguration();
                ConfigurationManager.InitializeConfiguration(config);

                ItemsGeneratorManager.Initialize(new ItemsGenerator(
                    config.ItemGenerator,
                    imageStorage,
                    new AncientSpellsProvider()));

                DialogsManager.Initialize(provider.GetRequiredService<IDialogsProvider>());

                GlyphsConverterManager.Initialize(new GlyphsConverter());

                DungeonMapGenerator.Initialize(imageStorage, provider.GetRequiredService<ILoggerFactory>(), Settings.Current.DebugWriteMapToFile);

                _logger.LogInformation("Starting the game.");

                CurrentGame.Initialize(provider.GetRequiredService<ILoggerFactory>());

                _game = provider.GetRequiredService<CodeMagicGame>();
                _game.Run();

                _logger.LogInformation("Closing the game.");
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Unexpected error.");
            }
        }

        internal static void Exit()
        {
            _game.Exit();
        }

        private static LogEventLevel GetLogLevel()
        {
            var stringValue = Settings.Current.LogLevel;
            if (Enum.TryParse(stringValue, true, out LogEventLevel level))
            {
                return level;
            }

            return DefaultLogLevel;
        }
    }
}
