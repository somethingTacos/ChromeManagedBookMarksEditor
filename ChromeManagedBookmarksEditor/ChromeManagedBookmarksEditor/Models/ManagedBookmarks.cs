using Newtonsoft.Json;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ChromeManagedBookmarksEditor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ManagedBookmarks : ReactiveObject
    {
        private string _RootName = "";

        [JsonProperty("toplevel_name")]
        public string RootName
        {
            get => _RootName;
            set => this.RaiseAndSetIfChanged(ref _RootName, value);
        }

        public ObservableCollection<Folder>  Folders { get; set; } = new ObservableCollection<Folder>();

        public ObservableCollection<Bookmark>  Bookmarks { get; set; } = new ObservableCollection<Bookmark>();
    }
}
