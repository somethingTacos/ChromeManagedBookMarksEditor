using ReactiveUI;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(IScreen Host) : base(Host)
        {
        }

        public async Task SaveAndCloseCommand()
        {
            NavigateBack();
        }
    }
}
