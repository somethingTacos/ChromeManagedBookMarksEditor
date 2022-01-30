using Newtonsoft.Json;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ChromeManagedBookmarksEditor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ManagedBookmarks
    {
        public string SaveFileName = "";

        [JsonProperty("toplevel_name")]
        public string RootName = "";

        public Folder RootFolder = new Folder();
    }
}
