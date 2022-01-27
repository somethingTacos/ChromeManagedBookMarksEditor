using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.ViewModels;
using Splat;

namespace ChromeManagedBookmarksEditor.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            WindowNotificationManager notificationManager = new WindowNotificationManager(this);

            SettingsHelper settingsHelper = new SettingsHelper(notificationManager);

            Locator.CurrentMutable.RegisterConstant(notificationManager);

            Locator.CurrentMutable.RegisterConstant(settingsHelper);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
