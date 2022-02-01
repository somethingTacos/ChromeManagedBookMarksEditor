using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ChromeManagedBookmarksEditor.Models;
using System.Windows.Input;

namespace ChromeManagedBookmarksEditor.CustomControls
{
    public partial class BookmarkControl : UserControl
    {
        public BookmarkControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<Bookmark> BookmarkItemProperty =
            AvaloniaProperty.Register<BookmarkControl, Bookmark>(nameof(BookmarkItem));

        public Bookmark BookmarkItem
        {
            get => GetValue(BookmarkItemProperty);
            set => SetValue(BookmarkItemProperty, value);
        }

        public static readonly StyledProperty<ICommand> RemoveCommandProperty =
            AvaloniaProperty.Register<BookmarkControl, ICommand>(nameof(RemoveCommand));

        public ICommand RemoveCommand
        {
            get => GetValue(RemoveCommandProperty);
            set => SetValue(RemoveCommandProperty, value);
        }

        public static readonly StyledProperty<bool> IsDragDropAllowedProperty =
            AvaloniaProperty.Register<BookmarkControl, bool>(nameof(IsDragDropAllowed));

        public bool IsDragDropAllowed
        {
            get => GetValue(IsDragDropAllowedProperty);
            set => SetValue(IsDragDropAllowedProperty, value);
        }
    }
}
