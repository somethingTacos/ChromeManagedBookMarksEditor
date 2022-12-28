using ChromeManagedBookmarksEditor.Models;

namespace ChromeManagedBookmarksEditor.Interfaces
{
    public interface IChild
    {
        public string Name { get; set; }
        public Folder? Parent { get; set; }
    }
}
