using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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

            Locator.CurrentMutable.RegisterConstant(notificationManager);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
