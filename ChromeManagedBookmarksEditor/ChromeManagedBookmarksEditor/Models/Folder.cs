using Newtonsoft.Json;
using ReactiveUI;
using System.Collections.Generic;

namespace ChromeManagedBookmarksEditor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Folder : ReactiveObject
    {
        private string _Name = "";

        [JsonProperty("name")]
        public string Name
        {
            get => _Name;
            set => this.RaiseAndSetIfChanged(ref _Name, value);
        }

        [JsonProperty("children")]
        public List<object> Children { get; set; } = new List<object>();
    }
}
