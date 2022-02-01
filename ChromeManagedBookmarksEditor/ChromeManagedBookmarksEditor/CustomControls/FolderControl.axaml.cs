using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ChromeManagedBookmarksEditor.Models;
using ReactiveUI;
using System.ComponentModel;
using System.Windows.Input;

namespace ChromeManagedBookmarksEditor.CustomControls
{
    public partial class FolderControl : UserControl
    {
        public FolderControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<Folder> FolderItemProperty =
            AvaloniaProperty.Register<FolderControl, Folder>(nameof(FolderItem));

        public Folder FolderItem
        {
            get => GetValue(FolderItemProperty);
            set => SetValue(FolderItemProperty, value);
        }

        public static readonly StyledProperty<ICommand> AddFolderCommandProperty =
            AvaloniaProperty.Register<FolderControl, ICommand>(nameof(AddFolderCommand));

        public ICommand AddFolderCommand
        {
            get => GetValue(AddFolderCommandProperty);
            set => SetValue(AddFolderCommandProperty, value);
        }

        public static readonly StyledProperty<ICommand> AddLinkCommandProperty =
            AvaloniaProperty.Register<FolderControl, ICommand>(nameof(AddLinkCommand));

        public ICommand AddLinkCommand
        {
            get => GetValue(AddLinkCommandProperty);
            set => SetValue(AddLinkCommandProperty, value);
        }

        public static readonly StyledProperty<ICommand> RemoveCommandProperty =
            AvaloniaProperty.Register<FolderControl, ICommand>(nameof(RemoveCommand));

        public ICommand RemoveCommand
        {
            get => GetValue(RemoveCommandProperty);
            set => SetValue(RemoveCommandProperty, value);
        }

        public static readonly StyledProperty<bool> IsDragDropAllowedProperty =
            AvaloniaProperty.Register<FolderControl, bool>(nameof(IsDragDropAllowed));

        public bool IsDragDropAllowed
        {
            get => GetValue(IsDragDropAllowedProperty);
            set => SetValue(IsDragDropAllowedProperty, value);
        }
    }
}
