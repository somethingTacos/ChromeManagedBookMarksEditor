using ChromeManagedBookmarksEditor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class EditorViewModel : ViewModelBase
    {
        private ManagedBookmarks _Bookmarks;
        public ManagedBookmarks Bookmarks
        {
            get => _Bookmarks;
            set => this.RaiseAndSetIfChanged(ref _Bookmarks, value);
        }
        public EditorViewModel(IScreen Host, ManagedBookmarks bookmarks = null) : base(Host)
        {
            Bookmarks = bookmarks != null ? bookmarks : new ManagedBookmarks();
        }
    }
}
