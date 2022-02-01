using ChromeManagedBookmarksEditor.Interfaces;
using Newtonsoft.Json;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ChromeManagedBookmarksEditor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Folder : ReactiveObject, IChild
    {
        public bool IsRoot { get; set; } = false;

        public Folder? Parent { get; set; } = null;

        public Folder(Folder? Parent = null)
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

        [JsonProperty("children")]
        public ObservableCollection<object> Children { get; set; } = new ObservableCollection<object>();
    }
}
