using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;
using PropertyChanged;

namespace ChromeManagedBookmarksEditor.Model
{
    public class ManagedBookmarksModel { }

    [ImplementPropertyChanged]
    public class ManagedBookmarks
    {
        public string CurrentWorkingFolder { get; set; } = "";
        public string toplevel_name { get; set; } = "Root Folder";
        public ObservableCollection<Folder> Folders { get; set; } = new ObservableCollection<Folder>();
        public ObservableCollection<URL> URLs { get; set; } = new ObservableCollection<URL>();
        public IList Children
        {
            get
            {
                return new CompositeCollection()
                {
                    new CollectionContainer() { Collection = Folders },
                    new CollectionContainer() { Collection = URLs }
                };
            }
        }
    }

    [ImplementPropertyChanged]
    public class RootFolder
    {
        public string toplevel_name { get; set; } = "";
    }

    [ImplementPropertyChanged]
    public class ParsedFolders
    {
        public string Name { get; set; } = "";
        public JArray children { get; set; } = new JArray();
    }

    [ImplementPropertyChanged]
    public class Folder
    {
        public string Name { get; set; } = "";
        public ObservableCollection<Folder> folders { get; set; } = new ObservableCollection<Folder>();
        public ObservableCollection<URL> URLs { get; set; } = new ObservableCollection<URL>();
        public IList subFolderChildren
        {
            get
            {
                return new CompositeCollection()
                {
                    new CollectionContainer() { Collection = folders },
                    new CollectionContainer() { Collection = URLs }
                };
            }
        }

        public bool IsSelected { get; set; } = false;
    }

    [ImplementPropertyChanged]
    public class URL
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }

    [ImplementPropertyChanged]
    public class Info
    {
        public string Text { get; set; } = "";
    }

    [ImplementPropertyChanged]
    public class JSONCode
    {
        public string Code { get; set; } = "";
    }
}
