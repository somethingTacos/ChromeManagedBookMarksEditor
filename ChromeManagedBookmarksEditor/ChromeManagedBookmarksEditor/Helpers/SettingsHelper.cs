using Avalonia.Controls.Notifications;
using ChromeManagedBookmarksEditor.Models;
using System;
using System.IO;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public class SettingsHelper
    {
        public static string SettingsFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/ChromeManagedBookmarksEditor";

        public static string SettingsFilePath = $"{SettingsFolderPath}/settings.json";

        private WindowNotificationManager notificationManager;

        private bool CheckFolderAndFile()
        {
            Directory.CreateDirectory(SettingsFolderPath);

            return File.Exists(SettingsFilePath);
        }

        public Settings LoadSettings()
        {
            if (CheckFolderAndFile()) 
            {
                var result = JsonHelper.LoadFromFile<Settings>(SettingsFilePath);

                if (result.Succeeded && result.Data != null)
                {
                    notificationManager.Show(new Notification("", "Settings Loaded", NotificationType.Success));

                    return (Settings)result.Data;
                }

                notificationManager.Show(new Notification("Error Loading Settings", result.ErrorMessage, NotificationType.Error));

                return null;
            }

            notificationManager.Show(new Notification(
                "No Settings Found",
                "A new settings file will be created when you edit your settings or try to save a json file.",
                NotificationType.Information,
                TimeSpan.FromSeconds(10)));

            return null;
        }

        public void SaveSettings(Settings Settings)
        {
            CheckFolderAndFile();

            var result = JsonHelper.SaveToFile<Settings>(Settings, SettingsFilePath);

            if(result.Succeeded)
            {
                notificationManager.Show(new Notification("", "Settings Saved", NotificationType.Success));
                return;
            }

            notificationManager.Show(new Notification("Error Saving Settings", result.ErrorMessage, NotificationType.Error));
        }

        public SettingsHelper(WindowNotificationManager manager)
        {
            notificationManager = manager;
        }
    }
}
