using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CodeMagic.Game;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.UI.Mono.Drawing.ImageProviding;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.GameProcess;
using CodeMagic.UI.Mono.Saving;
using CodeMagic.UI.Mono.Views;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using CodeMagic.Core.Common;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.UI.Mono.Extension.Glyphs;
using Microsoft.Extensions.Logging;
using CodeMagic.Core.Game;

namespace CodeMagic.UI.Mono;

[ExcludeFromCodeCoverage]
internal class Startup
{
    private const string LogFilePath = @".\log_.txt";
    private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;

    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = GameConfigurator.LoadConfiguration();
        ConfigurationManager.InitializeConfiguration(configuration);

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

        // Configuration
        services.AddSingleton(configuration.ItemGenerator);

        // Services
        services.AddSingleton<IAncientSpellsProvider, AncientSpellsProvider>();
        services.AddSingleton<IItemsGenerator, ItemsGenerator>();
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

        services.AddSingleton<ICodeMagicGame, CodeMagicGame>();
    }

    public async Task Initialize(IServiceProvider provider)
    {
        var imageStorage = provider.GetRequiredService<IImagesStorage>();
        await imageStorage.Load();

#if DEBUG
        PerformanceMeter.Initialize(@".\Performance.log");
#endif

        ItemsGeneratorManager.Initialize(provider.GetRequiredService<IItemsGenerator>());

        DialogsManager.Initialize(provider.GetRequiredService<IDialogsProvider>());

        GlyphsConverterManager.Initialize(new GlyphsConverter());

        DungeonMapGenerator.Initialize(imageStorage, provider.GetRequiredService<ILoggerFactory>(), Settings.Current.DebugWriteMapToFile);

        CurrentGame.Initialize(provider.GetRequiredService<ILoggerFactory>());
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
