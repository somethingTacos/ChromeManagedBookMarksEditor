using Avalonia;
using ChromeManagedBookmarksEditor.Interfaces;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.Models.Serializers;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class EditorViewModel : ViewModelBase
    {
        private bool _AllowNodeDragDrop = false;
        public bool AllowNodeDragDrop
        {
            get => _AllowNodeDragDrop;
            set => this.RaiseAndSetIfChanged(ref _AllowNodeDragDrop, value);
        }

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

        // lazy
        private string _SelectedSerializeOutputType = "";
        public string SelectedSerializeOutputType
        {
            get => _SelectedSerializeOutputType;
            set => this.RaiseAndSetIfChanged(ref _SelectedSerializeOutputType, value);
        }

        // more lazy
        // TODO: use BookmarkSerializedType with a converter
        public List<string> SerializeOutputTypes { get; } = new List<string>
        {
            "Json",
            "Html"
        };

        public EditorViewModel(IScreen Host, ManagedBookmarks? Bookmarks = null) : base(Host)
        {
            Folder root = new Folder() { Name = "Managed Bookmarks", IsRoot = true };

            if(Bookmarks != null)
            {
                root = Bookmarks.RootFolder;
                SaveFileName = Bookmarks.SaveFileName;
            }

            RootFolders.Add(root);

            originData = Bookmarks;

            SelectedSerializeOutputType = SerializeOutputTypes[0];
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
            if(string.IsNullOrWhiteSpace(JsonText))
            {
                SendNotification("", "Nothing to copy");
                return;
            }

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

        public void SortCommand()
        {
            SendNotification("Nope", "Not a thing yet WIP", Avalonia.Controls.Notifications.NotificationType.Warning);
        }

        public void SerializeCommand()
        {
            ManagedBookmarks bookmarks = new ManagedBookmarks();

            bookmarks.RootName = RootFolders[0].Name;

            bookmarks.RootFolder = RootFolders[0];

            IBookmarkSerializer? serializer = null;

            // TODO: remove lazy switch

            switch (SelectedSerializeOutputType)
            {
                case "Json":
                    {
                        serializer = new JsonBookmarkSerializer(bookmarks);
                        break;
                    }
                case "Html":
                    {
                        serializer = new HtmlBookmarkSerializer(bookmarks);
                        break;
                    }
            }

            if (serializer == null)
            {
                SendNotification("Serialization Error", "Something went wrong during serilization :(", Avalonia.Controls.Notifications.NotificationType.Error, TimeSpan.FromSeconds(5));
                return;
            }

            SaveFileName = SaveFileName == "" ? bookmarks.RootName : SaveFileName;

            JsonText = serializer.SerializeDataToFile(SaveFileName);

            if(JsonText == "")
            {
                SendNotification("No save folder", "Json file could not be automatically saved since the save folder isn't set.", Avalonia.Controls.Notifications.NotificationType.Warning, TimeSpan.FromSeconds(7));
                JsonText = serializer.SerializeData();
            }

            if (SaveFileName != "" && originData?.SaveFileName != "" && SaveFileName != originData?.SaveFileName)
            {
                string oldFilePath = Path.Join(Locator.Current.GetService<Settings>().SaveFolder, $"{originData?.SaveFileName}.json");

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            SendNotification("", $"{SaveFileName} saved", Avalonia.Controls.Notifications.NotificationType.Success, TimeSpan.FromSeconds(2));
        }
    }
}
