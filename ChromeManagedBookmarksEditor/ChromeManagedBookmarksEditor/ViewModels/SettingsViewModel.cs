using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.Models;
using ReactiveUI;
using Splat;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public Settings settings { get; set; } = new Settings();
        private SettingsHelper settingsHelper = Locator.Current.GetService<SettingsHelper>();

        public SettingsViewModel(IScreen Host) : base(Host)
        {
            Settings tmpSettings = Locator.Current.GetService<Settings>();

            settings.SaveFolder = tmpSettings.SaveFolder;
        }

        public void SaveAndCloseCommand()
        {
            var result = settingsHelper.Validate(settings);

            if (!result.Succeeded)
            {
                SendNotification("Settings Validation Failed", result.ErrorMessage, Avalonia.Controls.Notifications.NotificationType.Error);
                return;
            }

            settingsHelper.SaveSettings(settings);

            Locator.Current.GetService<Settings>().SaveFolder = settings.SaveFolder;

            NavigateBack();
        }

        public void CancelCommand()
        {
            NavigateBack();
        }
    }
}
