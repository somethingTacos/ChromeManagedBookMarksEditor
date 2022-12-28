using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ChromeManagedBookmarksEditor.Interfaces;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.Models.Serializers;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class StartupMenuViewModel : ViewModelBase
    {
        private string _DataToLoad = "";
        public string DataToLoad
        {
            get => _DataToLoad;
            set => this.RaiseAndSetIfChanged(ref _DataToLoad, value);
        }

        private ObservableCollection<FileInfo> SavedFilesCollection { get; set; } = new ObservableCollection<FileInfo>();

        public StartupMenuViewModel(IScreen Host) : base(Host)
        {
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                LoadSavedFiles();
            });
        }

        private void LoadSavedFiles()
        {
            Task.Run(() =>
            {
                SavedFilesCollection.Clear();

                string? saveFolder = Locator.Current.GetService<Settings>()?.SaveFolder;

                if (string.IsNullOrWhiteSpace(saveFolder) || !Directory.Exists(saveFolder)) return;

                string[] files = Directory.GetFiles(saveFolder);

                foreach (string file in files)
                {
                    if (file.EndsWith(".json") || file.EndsWith(".html"))
                    {
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            SavedFilesCollection.Add(new FileInfo(file));
                        });
                    }
                }
            });
        }

        private void LoadBookmarksDataEditor(BookmarkSerializedType type, string data, bool FromFile = false)
        {
            IBookmarkSerializer? serializer = null;

            switch (type)
            {
                case BookmarkSerializedType.Json:
                    serializer = JsonBookmarkSerializer.FromJson(data, FromFile);
                    break;
                case BookmarkSerializedType.Html:
                    serializer = HtmlBookmarkSerializer.FromHtml(data, FromFile);
                    break;
                default:
                    break;
            }

            if(serializer == null)
            {
                SendNotification("Serilizer Load Error", "Something went wrong loading data from file :(", Avalonia.Controls.Notifications.NotificationType.Error);
                return;
            }

            ManagedBookmarks bookmarks = serializer.BuildData();

            if (bookmarks == null)
            {
                SendNotification("File Load Failed", "Failed to deserialize json data", Avalonia.Controls.Notifications.NotificationType.Error);
                return;
            }

            NavigateTo(new EditorViewModel(HostScreen, type, bookmarks));
        }

        public void LoadDataFromFile(object FilePath)
        {
            if (FilePath is string path)
            {

                switch (new FileInfo(path).Extension)
                {
                    case ".json":
                        LoadBookmarksDataEditor(BookmarkSerializedType.Json, path, true);
                        break;
                    case ".html":
                        LoadBookmarksDataEditor(BookmarkSerializedType.Html, path, true);
                        break;
                    default:
                        break;
                }
            }
        }

        public void StartNewCommand()
        {
            NavigateTo(new EditorViewModel(HostScreen, BookmarkSerializedType.Json));
        }

        public async Task BrowseCommand()
        {
            string? saveFolder = Locator.Current.GetService<Settings>()?.SaveFolder;

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Directory = saveFolder;

            dialog.AllowMultiple = false;

            dialog.Filters.Add(new FileDialogFilter() { Extensions = new SerializationOutputs().AvailableTypes });

            if(Application.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                string[]? result = await dialog.ShowAsync(desktop.MainWindow);

                if(result != null && result.Length > 0)
                {
                    LoadDataFromFile(result[0]);
                }
            }
        }

        public void LoadDataCommand(object data)
        {
            if (data is string text)
            {

                if (string.IsNullOrWhiteSpace(text))
                {
                    SendNotification("", "No data to load", Avalonia.Controls.Notifications.NotificationType.Warning);
                    return;
                }

                if (text.StartsWith("<!DOCTYPE NETSCAPE-Bookmark-file-1>"))
                {
                    LoadBookmarksDataEditor(BookmarkSerializedType.Html, text);
                    return;
                }

                LoadBookmarksDataEditor(BookmarkSerializedType.Json, text);
            }
        }
    }
}
