using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.Interfaces;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChromeManagedBookmarksEditor.Models
{
    public class SerializableBookmarks
    {
        private string SaveFileName = "";

        public List<object> Data { get; private set; } = new List<object>();

        public SerializableBookmarks(ManagedBookmarks ManagedBookmarks)
        {
            Data.Add(ManagedBookmarks);

            Data.AddRange(ManagedBookmarks.RootFolder.Children);
        }

        public SerializableBookmarks(string Json, bool FromFile = false)
        {
            var result = FromFile ? JsonHelper.LoadFromFile<object[]>(Json) : JsonHelper.Deserialize<object[]>(Json);

            if (FromFile) SaveFileName = new FileInfo(Json).Name.Replace(".json", "");

            if(result.Succeeded && result.HasData && result.Data is object[] data)
            {
                var conversionResult = JsonHelper.ConvertDataToTypes(data);

                if(conversionResult.Succeeded && conversionResult.HasData && conversionResult.Data is object[] convertedData)
                {
                    Data = convertedData.ToList();
                }
            }
        }

        public string SerializeData()
        {
            return JsonHelper.Serialize(Data);
        }

        /// <summary>
        /// Serializes and saves the managedbookmarks data to a file.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>An unformatterd json string of the serialized data or an empty string</returns>
        public string SerializeDataToFile(string FileName)
        {
            if (Data.Count == 0) return "";

            string saveFolder = Locator.Current.GetService<Settings>()?.SaveFolder ?? "";

            if(saveFolder == "")
            {
                return "";
            }

            string filePath = Path.Join(saveFolder, $"{FileName}.json");

            var result = JsonHelper.SaveToFile(Data, filePath);

            if (result.Succeeded && result.HasData && result.Data is string json)
            {
                return json;
            }

            return "";
        }

        /// <summary>
        /// Builds the data object array into a ManagedBookmarks object
        /// </summary>
        /// <returns>a <see cref="ManagedBookmarks"/> object or null</returns>
        public ManagedBookmarks BuildData()
        {
            if (Data.Count == 0) return null;

            if (Data[0] is ManagedBookmarks mbm)
            {
                mbm.RootFolder.Name = mbm.RootName;

                mbm.SaveFileName = SaveFileName;

                foreach(var item in Data)
                {
                    if(item is IChild child)
                    {
                        child.Parent = mbm.RootFolder;
                        mbm.RootFolder.Children.Add(child);
                    }
                }

                return mbm;
            }

            return null;
        }
    }
}
