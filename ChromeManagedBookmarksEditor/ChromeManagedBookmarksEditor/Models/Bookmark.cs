using ChromeManagedBookmarksEditor.Interfaces;
using Newtonsoft.Json;
using ReactiveUI;

namespace ChromeManagedBookmarksEditor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Bookmark : ReactiveObject, IChild
    {
        public Folder? Parent { get; set; } = null;

        public Bookmark(Folder? Parent = null)
        {
            this.Parent = Parent;
        }

        private string _Name = "";

        [JsonProperty("name")]
        public string Name
        {
            get => _Name;
            set => this.RaiseAndSetIfChanged(ref _Name, value);
        }

        private string _Url = "";

        [JsonProperty("url")]
        public string Url
        {
            get => _Url;
            set => this.RaiseAndSetIfChanged(ref _Url, value);
        }

    }
}
