using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.Interfaces;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChromeManagedBookmarksEditor.Models.Serializers
{
    public class JsonBookmarkSerializer : BookmarkSerializerBase, IBookmarkSerializer
    {
        private JsonBookmarkSerializer(string SaveFileName, List<object> Data) : base(SaveFileName, Data)
        {
        }

        public JsonBookmarkSerializer(ManagedBookmarks ManagedBookmarks) : base(ManagedBookmarks)
        {
        }

        public static JsonBookmarkSerializer? FromJson(string Json, bool FromFile = false)
        {
            var Data = new List<object>();
            string saveFileName = "";

            var result = FromFile ? JsonHelper.LoadFromFile<object[]>(Json) : JsonHelper.Deserialize<object[]>(Json);

            if (FromFile)
            {
                saveFileName = new FileInfo(Json).Name.Replace(".json", "");
            }

            if (result.Succeeded && result.HasData && result.Data is object[] data)
            {
                var conversionResult = JsonHelper.ConvertDataToTypes(data);

                if (conversionResult.Succeeded && conversionResult.HasData && conversionResult.Data is object[] convertedData)
                {
                    Data = convertedData.ToList();

                    return new JsonBookmarkSerializer(saveFileName, Data);
                }
            }

            return null;
        }

        public string SerializeData()
        {
            return JsonHelper.Serialize(Data);
        }

        public string SerializeDataToFile(string FileName)
        {
            if (Data.Count == 0) return "";

            string saveFolder = Locator.Current.GetService<Settings>()?.SaveFolder ?? "";

            if (saveFolder == "")
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
    }
}
