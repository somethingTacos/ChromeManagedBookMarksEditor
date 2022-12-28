using System;
using System.Collections.Generic;
using BookmarksManager;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.Models.Results;
using System.IO;
using ChromeManagedBookmarksEditor.Interfaces;

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
                    continue;
                }

                if (child is Bookmark bookmark)
                {
                    netscapeFolder.Add(new BookmarkLink(bookmark.Url, bookmark.Name));
                }
            }

            return netscapeFolder;
        }

        private static Folder ConvertToFolder(BookmarkFolder folder, Folder parent = null)
        {
            Folder managedFolder = new Folder(parent)
            {
                Name = folder.Title
            };

            foreach (var child in folder)
            {
                if (child is BookmarkLink link)
                {
                    managedFolder.Children.Add(new Bookmark(parent)
                    {
                        Name = link.Title,
                        Url = link.Url
                    });
                }

                if (child is BookmarkFolder subFolder)
                {
                    managedFolder.Children.Add(ConvertToFolder(subFolder, parent));
                }
            }

            return managedFolder;
        }

        public static string Serialize(object[] bookmarks)
        {
            return new NetscapeBookmarksWriter(SerializeToContainer(bookmarks)).ToString();
        }

        public static BookmarkFolder SerializeToContainer(object[] bookmarks)
        {
            var rootFolder = new BookmarkFolder();

            foreach (var bookmark in bookmarks)
            {
                if (bookmark is ManagedBookmarks managedBookmarks)
                {
                    rootFolder.Title = managedBookmarks.RootName;
                    continue;
                }

                if (bookmark is Bookmark link)
                {
                    rootFolder.Add(new BookmarkLink(link.Name, link.Url));
                    continue;
                }

                if (bookmark is Folder folder)
                {
                    rootFolder.Add(ConvertToNetScapeFolder(folder));
                }
            }

            var container = new BookmarkFolder()
            {
                rootFolder
            };

            return container;
        }

        public static GenericResult Deserialize(string html)
        {
            var netscapeBookmarks = new NetscapeBookmarksReader().Read(html);

            if (netscapeBookmarks == null) return GenericResult.FromError("Failed to deserialize bookmarks");

            List<object> data = new List<object>();

            if (netscapeBookmarks == null || netscapeBookmarks.Count == 0) return GenericResult.FromError("Bookmarks are null or have no child items");

            data.Add(new ManagedBookmarks()
            {
                RootName = netscapeBookmarks[0].Title
            });

            if (netscapeBookmarks[0] is BookmarkFolder rootFolder)
            {
                foreach (var child in rootFolder)
                {
                    if (child is BookmarkLink link)
                    {
                        data.Add(new Bookmark()
                        {
                            Name = link.Title,
                            Url = link.Url
                        });
                    }

                    if (child is BookmarkFolder subFolder)
                    {
                        data.Add(ConvertToFolder(subFolder));
                    }
                }
            }

            return GenericResult.FromSuccess("", data.ToArray());
        }

        public static GenericResult SaveToFile(List<object> data, string filePath)
        {
            try
            {
                var container = SerializeToContainer(data.ToArray());

                using (var file = File.OpenWrite(filePath))
                {
                    var writer = new NetscapeBookmarksWriter(container);

                    writer.Write(file);

                    return GenericResult.FromSuccess("", writer.ToString());
                }
            }
            catch (Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }

        public static GenericResult LoadFromFile(string filePath)
        {
            try
            {
                string htmlData = File.ReadAllText(filePath);

                return Deserialize(htmlData);
            }
            catch (Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }
    }
}
