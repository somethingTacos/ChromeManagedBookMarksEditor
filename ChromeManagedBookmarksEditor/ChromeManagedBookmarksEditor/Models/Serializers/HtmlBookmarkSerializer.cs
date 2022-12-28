using ChromeManagedBookmarksEditor.Helpers;
using ChromeManagedBookmarksEditor.Interfaces;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChromeManagedBookmarksEditor.Models.Serializers
{
    public class HtmlBookmarkSerializer : BookmarkSerializerBase, IBookmarkSerializer
    {
        private HtmlBookmarkSerializer(string saveFileName, List<object> data) : base(saveFileName, data)
        {
        }

        public HtmlBookmarkSerializer(ManagedBookmarks managedBookmarks) : base(managedBookmarks)
        {
        }

        public static HtmlBookmarkSerializer? FromHtml(string html, bool FromFile = false)
        {
            string saveFileName = "";

            var result = FromFile ? HtmlHelper.LoadFromFile(html) : HtmlHelper.Deserialize(html);

            if (FromFile)
            {
                saveFileName = new FileInfo(html).Name.Replace(".html", "");
            }

            if (result.Succeeded && result.HasData && result.Data is object[] data)
            {
                return new HtmlBookmarkSerializer(saveFileName, data.ToList());
            }

            return null;
        }


        public string SerializeData()
        {
            return HtmlHelper.Serialize(Data.ToArray());
        }

        public string SerializeDataToFile(string FileName)
        {
            if (Data.Count == 0) return "";

            string saveFolder = Locator.Current.GetService<Settings>()?.SaveFolder ?? "";

            if (saveFolder == "")
            {
                return "";
            }

            string filePath = Path.Join(saveFolder, $"{FileName}.html");

            var result = HtmlHelper.SaveToFile(Data, filePath);

            if (result.Succeeded && result.HasData && result.Data is string html)
            {
                return html;
            }

            return "";
        }
    }
}
