using ChromeManagedBookmarksEditor.Models;

namespace ChromeManagedBookmarksEditor.Interfaces
{
    public interface IBookmarkSerializer
    {
        /// <summary>
        /// Serialize bookmark data
        /// </summary>
        /// <returns>The serialized data as a string</returns>
        public string SerializeData();

        /// <summary>
        /// Serialize bookmark data and save it to a file
        /// </summary>
        /// <param name="FileName">The name of the file without an extension</param>
        /// <returns>The serialized data as a string</returns>
        public string SerializeDataToFile(string FileName);

        /// <summary>
        /// Build the data into a ManagedBookmarks object
        /// </summary>
        /// <returns>A ManagedBookmarks object</returns>
        public ManagedBookmarks BuildData();
    }
}
