using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChromeManagedBookmarksEditor.Model;

namespace ChromeManagedBookmarksEditor.ViewModel
{
    public class CurrentFolderViewModel
    {
        public ManagedBookmarks workingBookmarks { get; set; }
        public CurrentFolderViewModel()
        {
            workingBookmarks = StaticManagedBookmarks.BookmarksCollection[0];
        }
    }
}
