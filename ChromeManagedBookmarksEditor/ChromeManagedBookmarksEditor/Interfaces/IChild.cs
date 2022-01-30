using ChromeManagedBookmarksEditor.Models;

namespace ChromeManagedBookmarksEditor.Interfaces
{
    public interface IChild
    {
        public Folder? Parent { get; set; }
    }
}
