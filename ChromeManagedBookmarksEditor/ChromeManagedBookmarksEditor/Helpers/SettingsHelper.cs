using Avalonia.Controls.Notifications;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.Models.Results;
using System;
using System.IO;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public class SettingsHelper
    {
        public static string SettingsFolderPath = Path.Join($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}","ChromeManagedBookmarksEditor");

        public static string SettingsFilePath = Path.Join($"{SettingsFolderPath}","settings.json");

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
                    notificationManager.Show(new Notification("", "Settings Loaded", NotificationType.Success, TimeSpan.FromSeconds(2)));

                    return (Settings)result.Data;
                }

                notificationManager.Show(new Notification("Error Loading Settings", result.ErrorMessage, NotificationType.Error));

                return null;
            }

            notificationManager.Show(new Notification(
                "No Settings Found",
                "A new settings file will be created when you edit your settings.",
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

        public GenericResult Validate(Settings settings)
        {
            if (settings == null) return GenericResult.FromError("Settings is null");

            if (!Directory.Exists(settings.SaveFolder)) return GenericResult.FromError("Save folder path could not be found");

            return GenericResult.FromSuccess();
        }

        public SettingsHelper(WindowNotificationManager manager)
        {
            notificationManager = manager;
        }
    }
}
