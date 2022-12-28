using Avalonia;
using ChromeManagedBookmarksEditor.Interfaces;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.Models.Serializers;
using DynamicData;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private string _DataText = "";
        public string DataText
        {
            get => _DataText;
            set => this.RaiseAndSetIfChanged(ref _DataText, value);
        }

        public ObservableCollection<Folder> RootFolders { get; set; } = new ObservableCollection<Folder>();

        private OutputType _SelectedSerializeOutputType = OutputType.Json;
        public OutputType SelectedSerializeOutputType
        {
            get => _SelectedSerializeOutputType;
            set => this.RaiseAndSetIfChanged(ref _SelectedSerializeOutputType, value);
        }

        private SortType _SelectedSortType = SortType.All;
        public SortType SelectedSortType
        {
            get => _SelectedSortType;
            set => this.RaiseAndSetIfChanged(ref _SelectedSortType, value);
        }

        public List<OutputType> Outputs { get; } = Enum.GetValues(typeof(OutputType)).Cast<OutputType>().ToList();

        public List<SortType> Sorts { get; } = Enum.GetValues(typeof(SortType)).Cast<SortType>().ToList();

        public EditorViewModel(IScreen Host, OutputType outputType, ManagedBookmarks? Bookmarks = null) : base(Host)
        {
            Folder root = new Folder() { Name = "Managed Bookmarks", IsRoot = true };

            if(Bookmarks != null)
            {
                root = Bookmarks.RootFolder;
                SaveFileName = Bookmarks.SaveFileName;
            }

            RootFolders.Add(root);

            originData = Bookmarks;

            SelectedSerializeOutputType = outputType;
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
            if(string.IsNullOrWhiteSpace(DataText))
            {
                SendNotification("", "Nothing to copy");
                return;
            }

            if (Application.Current?.Clipboard != null)
            {
                SendNotification("", "Copied!", Avalonia.Controls.Notifications.NotificationType.Success, TimeSpan.FromSeconds(2));
                await Application.Current.Clipboard.SetTextAsync(DataText);
            }
        }

        public void BackToMenuCommand()
        {
            NavigateTo(new StartupMenuViewModel(HostScreen));
        }

        public void SortFolder(Folder folder, SortType sort)
        {
            List<object> sortedList = new List<object>();
            List<IChild> children = folder.Children.Cast<IChild>().ToList();

            void OrderFolder()
            {
                var folders = children.Where(x => x is Folder).ToList();
                children.Remove(folders);

                children.AddRange(folders.OrderBy(x => x.Name).ToList());
            }

            void OrderLinks()
            {
                var links = children.Where(x => x is Bookmark).ToList();
                children.Remove(links);

                children.AddRange(links.OrderBy(x => x.Name).ToList());
            }

            switch (SelectedSortType)
            {
                case SortType.FoldersOnly:
                    OrderFolder();

                    var links = children.Where(x => x is Bookmark).ToList();

                    children.Remove(links);
                    children.AddRange(links);

                    break;
                case SortType.UrlsOnly:
                    OrderLinks();
                    break;
                case SortType.All:
                    OrderFolder();
                    OrderLinks();
                    break;
                default:
                    break;
            }

            folder.Children.Clear();

            foreach(var child in children)
            {
                if(child is Folder subFolder)
                {
                    SortFolder(subFolder, sort);
                    folder.Children.Add(subFolder);
                    continue;
                }

                folder.Children.Add(child);
            }
        }

        public void SortCommand()
        {
            SortFolder(RootFolders[0], SelectedSortType);
        }

        public void SerializeCommand()
        {
            ManagedBookmarks bookmarks = new ManagedBookmarks();

            bookmarks.RootName = RootFolders[0].Name;

            bookmarks.RootFolder = RootFolders[0];

            IBookmarkSerializer? serializer = null;

            switch (SelectedSerializeOutputType)
            {
                case OutputType.Json:
                    {
                        serializer = new JsonBookmarkSerializer(bookmarks);
                        break;
                    }
                case OutputType.Html:
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

            DataText = serializer.SerializeDataToFile(SaveFileName);

            if(DataText == "")
            {
                SendNotification("No save folder", "File could not be automatically saved since the save folder isn't set.", Avalonia.Controls.Notifications.NotificationType.Warning, TimeSpan.FromSeconds(7));
                DataText = serializer.SerializeData();
            }

            if (SaveFileName != "" && originData?.SaveFileName != "" && SaveFileName != originData?.SaveFileName)
            {
                string oldFilePath = Path.Join(Locator.Current.GetService<Settings>().SaveFolder, $"{originData?.SaveFileName}.{SelectedSerializeOutputType.ToString().ToLower()}");

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            SendNotification("", $"{SaveFileName}.{SelectedSerializeOutputType.ToString().ToLower()} saved", Avalonia.Controls.Notifications.NotificationType.Success, TimeSpan.FromSeconds(2));
        }
    }
}
