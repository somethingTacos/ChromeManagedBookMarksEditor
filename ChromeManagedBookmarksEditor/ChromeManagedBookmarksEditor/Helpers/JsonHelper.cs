using ChromeManagedBookmarksEditor.Models.Results;
using System;
using System.IO;
using System.Text.Json;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public static class JsonHelper
    {
        public static GenericResult SaveToFile<T>(T Data, string FilePath) where T : class
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(Data, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(jsonData, FilePath);

                return GenericResult.FromSuccess();
            }
            catch(Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }

        public static GenericResult LoadFromFile<T>(string FilePath) where T : class
        {
            try
            {
                string jsonData = File.ReadAllText(FilePath);

                T Data = JsonSerializer.Deserialize<T>(jsonData);

                return GenericResult.FromSuccess("", Data);
            }
            catch(Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }
    }
}
