using ChromeManagedBookmarksEditor.Interfaces;
using System;
using System.Collections.Generic;

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

        public static HtmlBookmarkSerializer FromHtml(string html, bool FromFile = false)
        {
            throw new NotImplementedException();
        }


        public string SerializeData()
        {
            throw new NotImplementedException();
        }

        public string SerializeDataToFile(string FileName)
        {
            throw new NotImplementedException();
        }
    }
}
