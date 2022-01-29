using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChromeManagedBookmarksEditor.ViewModels;

namespace ChromeManagedBookmarksEditor.Views
{
    public partial class EditorView : ReactiveUserControl<EditorViewModel>
    {
        public EditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
