using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChromeManagedBookmarksEditor.ViewModels;

namespace ChromeManagedBookmarksEditor.Views
{
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
