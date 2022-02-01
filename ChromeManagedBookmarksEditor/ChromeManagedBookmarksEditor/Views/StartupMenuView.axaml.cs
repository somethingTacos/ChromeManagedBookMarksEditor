using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChromeManagedBookmarksEditor.ViewModels;
using ReactiveUI;
using System;

namespace ChromeManagedBookmarksEditor.Views
{
    public partial class StartupMenuView : ReactiveUserControl<StartupMenuViewModel>
    {
        public StartupMenuView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
