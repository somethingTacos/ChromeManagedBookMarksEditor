using ChromeManagedBookmarksEditor.Interfaces;
using System.Collections.Generic;

namespace ChromeManagedBookmarksEditor.Models.Serializers
{
    public class BookmarkSerializerBase
    {
        private string _SaveFileName = "";

        public List<object> Data { get; private set; } = new List<object>();

        public BookmarkSerializerBase(string saveFileName, List<object> data)
        {
            _SaveFileName = saveFileName;
            Data = data;
        }

        public BookmarkSerializerBase(ManagedBookmarks managedBookmarks)
        {
            Data.Add(managedBookmarks);

            Data.AddRange(managedBookmarks.RootFolder.Children);
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

                mbm.SaveFileName = _SaveFileName;

                foreach (var item in Data)
                {
                    if (item is IChild child)
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
