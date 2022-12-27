using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookmarksManager;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.Models.Results;

namespace ChromeManagedBookmarksEditor.Helpers
{
    
    public static class HtmlHelper
    {
        private static BookmarkFolder ConvertToNetScapeFolder(Folder folder)
        {
            var netscapeFolder = new BookmarkFolder(folder.Name);

            foreach (var child in folder.Children)
            {
                if (child is Folder subFolder)
                {
                    netscapeFolder.Add(ConvertToNetScapeFolder(subFolder));
                }

                if (child is Bookmark bookmark)
                {
                    netscapeFolder.Add(new BookmarkLink(bookmark.Url, bookmark.Name));
                }
            }

            return netscapeFolder;
        }

        private static List<object> ConvertToObjectList(BookmarkFolder folder)
        {
            List<object> data = new List<object>();

            foreach (var child in folder)
            {

            }

            return data;
        }

        public static string Serialize(ManagedBookmarks bookmarks)
        {
            var netscapeBookmarks = new BookmarkFolder()
            {
                ConvertToNetScapeFolder(bookmarks.RootFolder)
            };

            return new NetscapeBookmarksWriter(netscapeBookmarks).ToString();
        }

        public static GenericResult Deserialize(string Html)
        {
            var netscapeBookmarks = new NetscapeBookmarksReader().Read(Html);

            if (netscapeBookmarks == null) return GenericResult.FromError("Failed to deserialize bookmarks");

            var data = ConvertToObjectList(netscapeBookmarks);

            return GenericResult.FromSuccess("", data);
        }

        public static GenericResult SaveToFile()
        {
            throw new NotImplementedException();
        }

        public static GenericResult LoadFromFile()
        {
            throw new NotImplementedException();
        }
    }
}
