using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
