using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.Models;
using ReactiveUI;
using Splat;
using System.Reactive.Disposables;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IActivatableViewModel, IScreen
    {
        public ViewModelActivator Activator { get; set; } = new ViewModelActivator();

        public RoutingState Router { get; set; } = new RoutingState();

        private SettingsHelper settingsHelper => Locator.Current.GetService<SettingsHelper>();

        private Settings? settings;

        public MainWindowViewModel()
        {
            this.WhenActivated((CompositeDisposable d) =>
            {
                settings = settingsHelper.LoadSettings() ?? new Settings();

                Locator.CurrentMutable.RegisterConstant(settings);

                Router.NavigateAndReset.Execute(new StartupMenuViewModel(this));
            });
        }
    }
}
