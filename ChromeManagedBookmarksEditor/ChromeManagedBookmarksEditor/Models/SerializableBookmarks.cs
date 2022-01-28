using ChromeManagedBookmarksEditor.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ChromeManagedBookmarksEditor.Models
{
    public class SerializableBookmarks
    {
        public List<object> Data { get; private set; } = new List<object>();

        public SerializableBookmarks(ManagedBookmarks ManagedBookmarks)
        {
            Data.Add(ManagedBookmarks);

            foreach(Folder f in ManagedBookmarks.Folders)
            {
                Data.Add(f);
            }

            foreach (Bookmark b in ManagedBookmarks.Bookmarks)
            {
                Data.Add(b);
            }
        }

        public SerializableBookmarks(string FilePath)
        {
            var result = JsonHelper.LoadFromFile<object[]>(FilePath);

            if(result.Succeeded && result.HasData && result.Data is object[] data)
            {
                var conversionResult = JsonHelper.ConvertDataToTypes(data);

                if(conversionResult.Succeeded && conversionResult.HasData && conversionResult.Data is object[] convertedData)
                {
                    Data = convertedData.ToList();
                }
            }
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

            string filePath = Path.Join(saveFolder, FileName);

            var result = JsonHelper.SaveToFile(Data, filePath);

            if (result.Succeeded && result.HasData && result.Data is string json)
            {
                return json;
            }

            return "";
        }

        public ManagedBookmarks BuildData()
        {
            if (Data.Count == 0) return null;

            if (Data[0] is ManagedBookmarks mbm)
            {
                Folder[]? folders = Data.OfType<Folder>().ToArray();

                Bookmark[]? bookmarks = Data.OfType<Bookmark>().ToArray();

                mbm.Folders = new ObservableCollection<Folder>(folders ?? new Folder[0]);
                mbm.Bookmarks = new ObservableCollection<Bookmark>(bookmarks ?? new Bookmark[0]);

                return mbm;
            }

            return null;
        }
    }
}
