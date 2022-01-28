using ReactiveUI;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class StartupMenuViewModel : ViewModelBase
    {
        private string _JsonToLoad = "";
        public string JsonToLoad
        {
            get => _JsonToLoad;
            set => this.RaiseAndSetIfChanged(ref _JsonToLoad, value);
        }

        public StartupMenuViewModel(IScreen Host) : base(Host)
        {
        }

        public void StartNewCommand()
        {
            //TODO - Navigate to editor without any data for a fresh start
        }

        public async Task LoadFromFileCommand()
        {
            //TODO - pick/load json from file and call LoadJsonCommand to validate / move to editor

            //TODO - maybe change the interface to have a list of recent files, just to make things quicker.
            //     - and have a smaller 'browse' button to manually pick a file.
            //     - Maybe support drag and drop as well.
        }

        public async Task LoadJsonCommand()
        {
            if(string.IsNullOrWhiteSpace(JsonToLoad))
            {
                SendNotification("", "No Json to load", Avalonia.Controls.Notifications.NotificationType.Warning);
                return;
            }

            SendNotification("Sorry :(", "Not yet implemented", Avalonia.Controls.Notifications.NotificationType.Error);
            //TODO - convert json to data model, navigate to editor view if it is valid.
        }
    }
}
