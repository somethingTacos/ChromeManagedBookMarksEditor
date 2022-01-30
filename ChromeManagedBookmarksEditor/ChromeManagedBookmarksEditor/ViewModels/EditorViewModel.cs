using Avalonia;
using ChromeManagedBookmarksEditor.Interfaces;
using ChromeManagedBookmarksEditor.Models;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class EditorViewModel : ViewModelBase
    {
        private ManagedBookmarks? originData;

        private string _SaveFileName = "";
        public string SaveFileName
        {
            get => _SaveFileName;
            set => this.RaiseAndSetIfChanged(ref _SaveFileName, value);
        }

        private string _JsonText = "";
        public string JsonText
        {
            get => _JsonText;
            set => this.RaiseAndSetIfChanged(ref _JsonText, value);
        }

        public ObservableCollection<Folder> RootFolders { get; set; } = new ObservableCollection<Folder>();

        public EditorViewModel(IScreen Host, ManagedBookmarks? Bookmarks = null) : base(Host)
        {
            Folder root = new Folder() { Name = "Managed Bookmarks"};

            if(Bookmarks != null)
            {
                root = Bookmarks.RootFolder;
                SaveFileName = Bookmarks.SaveFileName;
            }

            RootFolders.Add(root);

            originData = Bookmarks;
        }

        public void AddLinkCommand(object sender)
        {
            if(sender is Folder folder)
            {
                folder.Children.Add(new Bookmark(folder));
            }
        }

        public void AddFolderCommand(object sender)
        {
            if(sender is Folder folder)
            {
                folder.Children.Add(new Folder(folder) { Name = "New Folder" });
            }
        }

        public void RemoveItemCommand(object sender)
        {
            if(sender == RootFolders[0])
            {
                SendNotification("", "Can't remove root folder", Avalonia.Controls.Notifications.NotificationType.Error);
                return;
            }

            if(sender is IChild child)
            {
                child.Parent?.Children.Remove(sender);
            }
        }

        public async Task CopyCommand()
        {
            if (Application.Current?.Clipboard != null)
            {
                SendNotification("", "Copied!", Avalonia.Controls.Notifications.NotificationType.Success, TimeSpan.FromSeconds(2));
                await Application.Current.Clipboard.SetTextAsync(JsonText);
            }
        }

        public void BackToMenuCommand()
        {
            NavigateTo(new StartupMenuViewModel(HostScreen));
        }

        public void SerializeCommand()
        {
            ManagedBookmarks bookmarks = new ManagedBookmarks();

            bookmarks.RootName = RootFolders[0].Name;

            bookmarks.RootFolder = RootFolders[0];

            SerializableBookmarks serializer = new SerializableBookmarks(bookmarks);

            SaveFileName = SaveFileName == "" ? bookmarks.RootName : SaveFileName;

            JsonText = serializer.SerializeDataToFile(SaveFileName);

            if(JsonText == "")
            {
                SendNotification("No save folder", "Json file could not be automatically saved since the save folder isn't set.", Avalonia.Controls.Notifications.NotificationType.Warning, TimeSpan.FromSeconds(7));
                JsonText = serializer.SerializeData();
            }
        }
    }
}
