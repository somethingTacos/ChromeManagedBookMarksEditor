using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.Models;
using ReactiveUI;
using Splat;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public Settings settings { get; set; } = Locator.Current.GetService<Settings>();
        private SettingsHelper settingsHelper = Locator.Current.GetService<SettingsHelper>();

        public SettingsViewModel(IScreen Host) : base(Host)
        {
        }

        public void SaveAndCloseCommand()
        {
            var result = settingsHelper.Validate(settings);

            if(!result.Succeeded)
            {
                SendNotification("Settings Validation Failed", result.ErrorMessage, Avalonia.Controls.Notifications.NotificationType.Error);
                return;
            }

            settingsHelper.SaveSettings(settings);

            NavigateBack();
        }
    }
}
