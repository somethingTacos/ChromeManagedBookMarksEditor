using ReactiveUI;
using System;
using System.Text.Json.Serialization;

namespace ChromeManagedBookmarksEditor.Models
{
    public class Settings : ReactiveObject
    {
        private string _Settings = "";

        [JsonPropertyName("save_folder")]
        public string SaveFolder
        {
            get => _Settings;
            set => this.RaiseAndSetIfChanged(ref _Settings, value);
        }
    }
}
