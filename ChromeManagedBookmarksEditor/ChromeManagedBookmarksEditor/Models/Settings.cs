using Newtonsoft.Json;
using ReactiveUI;

namespace ChromeManagedBookmarksEditor.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings : ReactiveObject
    {
        private string _Settings = "";

        [JsonProperty("save_folder")]
        public string SaveFolder
        {
            get => _Settings;
            set => this.RaiseAndSetIfChanged(ref _Settings, value);
        }
    }
}
