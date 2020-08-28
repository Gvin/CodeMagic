using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CodeMagic.Game.GameProcess;
using CodeMagic.UI.Mono.GameProcess;
using CodeMagic.UI.Mono.Saving;
using CodeMagic.UI.Mono.Views;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.GameProcess;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Mono
{
    public static class IoC
    {
        public static IWindsorContainer Container { get; private set; }

        public static void Configure()
        {
            Container = new WindsorContainer();
            Container.Register(
                Component.For<IApplicationController>().ImplementedBy<ApplicationController>().LifestyleSingleton(),

                // Views
                Component.For<ICheatsView>().ImplementedBy<CheatsView>().LifestyleTransient(),
                Component.For<ICustomInventoryView>().ImplementedBy<CustomInventoryView>().LifestyleTransient(),
                Component.For<IEditSpellView>().ImplementedBy<EditSpellView>().LifestyleTransient(),
                Component.For<IGameView>().ImplementedBy<GameView>().LifestyleTransient(),
                Component.For<IInGameMenuView>().ImplementedBy<InGameMenuView>().LifestyleTransient(),
                Component.For<ILevelUpView>().ImplementedBy<LevelUpView>().LifestyleTransient(),
                Component.For<ILoadSpellView>().ImplementedBy<LoadSpellView>().LifestyleTransient(),
                Component.For<IMainMenuView>().ImplementedBy<MainMenuView>().LifestyleTransient(),
                Component.For<IMainSpellsLibraryView>().ImplementedBy<MainSpellsLibraryView>().LifestyleTransient(),
                Component.For<IPlayerDeathView>().ImplementedBy<PlayerDeathView>().LifestyleTransient(),
                Component.For<IPlayerInventoryView>().ImplementedBy<PlayerInventoryView>().LifestyleTransient(),
                Component.For<IPlayerStatsView>().ImplementedBy<PlayerStatsView>().LifestyleTransient(),
                // Component.For<ISettingsView>().ImplementedBy<SettingsView>().LifestyleTransient(),
                Component.For<ISpellBookView>().ImplementedBy<SpellBookView>().LifestyleTransient(),
                Component.For<IWaitMessageView>().ImplementedBy<WaitMessageView>().LifestyleTransient(),

                // Services
                Component.For<IEditSpellService>().ImplementedBy<EditSpellService>(),
                Component.For<ISpellsLibraryService>().ImplementedBy<SpellsLibraryService>().LifestyleSingleton(),
                Component.For<ISettingsService>().Instance(Settings.Current),
                Component.For<IApplicationService>().ImplementedBy<ApplicationService>(),
                Component.For<ISaveService>().ImplementedBy<SaveService>(),
                Component.For<IGameManager>().UsingFactoryMethod(
                    kernel => new GameManager(
                        kernel.Resolve<ISaveService>(), 
                        kernel.Resolve<ISettingsService>().SavingInterval))
                    .LifestyleSingleton()
            );
        }
    }
}