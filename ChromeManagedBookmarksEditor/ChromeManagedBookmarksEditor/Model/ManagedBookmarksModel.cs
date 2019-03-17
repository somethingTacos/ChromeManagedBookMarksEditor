using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;
using PropertyChanged;

namespace LinkHierarchy2JSON.Model
{
    public class ManagedBookmarksModel { }

    [ImplementPropertyChanged]
    public class ManagedBookmarks
    {
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
    }

    [ImplementPropertyChanged]
    public class URL
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
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

// OLD DATA MODEL

//    public class ManagedBookmarks : INotifyPropertyChanged
//    {
//        private string _name;
//        private ObservableCollection<Folder> _folders;
//        private ObservableCollection<URL> _urls;

//        public string toplevel_name {
//            get
//            {
//                return _name;
//            }
//            set
//            {
//                _name = value;
//                RaisePropertyChanged("toplevel_name");
//            }
//        }
//        public ObservableCollection<Folder> Folders
//        {
//            get
//            {
//                return _folders;
//            }
//            set
//            {
//                _folders = value;
//                RaisePropertyChanged("Folders");
//            }
//        }

//        public ObservableCollection<URL> URLs
//        {
//            get
//            {
//                return _urls;
//            }
//            set
//            {
//                _urls = value;
//                RaisePropertyChanged("URLs");
//            }
//        }

//        public IList Children
//        {
//            get
//            {
//                return new CompositeCollection()
//                {
//                    new CollectionContainer() { Collection = Folders },
//                    new CollectionContainer() { Collection = URLs }
//                };
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class RootFolder : INotifyPropertyChanged
//    {
//        private string _toplevelName;

//        public string toplevel_name
//        {
//            get
//            {
//                return _toplevelName;
//            }
//            set
//            {
//                _toplevelName = value;
//                RaisePropertyChanged("toplevel_name");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class ParsedFolders : INotifyPropertyChanged
//    {
//        private string _name;
//        private JArray _children;

//        public string Name
//        {
//            get
//            {
//                return _name;
//            }
//            set
//            {
//                _name = value;
//                RaisePropertyChanged("Name");
//            }
//        }

//        public JArray children
//        {
//            get
//            {
//                return _children;
//            }
//            set
//            {
//                _children = value;
//                RaisePropertyChanged("children");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class Folder : INotifyPropertyChanged
//    {
//        private string _name;
//        private ObservableCollection<Folder> _folders;
//        private ObservableCollection<URL> _urls;

//        public string Name
//        {
//            get
//            {
//                return _name;
//            }
//            set
//            {
//                _name = value;
//                RaisePropertyChanged("Name");
//            }
//        }

//        public ObservableCollection<Folder> folders
//        {
//            get
//            {
//                return _folders;
//            }
//            set
//            {
//                _folders = value;
//                RaisePropertyChanged("Folders");
//            }
//        }

//        public ObservableCollection<URL> URLs
//        {
//            get
//            {
//                return _urls;
//            }
//            set
//            {
//                _urls = value;
//                RaisePropertyChanged("URLs");
//            }
//        }

//        public IList subFolderChildren
//        {
//            get
//            {
//                return new CompositeCollection()
//                {
//                    new CollectionContainer() { Collection = folders },
//                    new CollectionContainer() { Collection = URLs }
//                };
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class URL : INotifyPropertyChanged
//    {
//        private string _name;
//        private string _url;

//        public string Name
//        {
//            get
//            {
//                return _name;
//            }
//            set
//            {
//                _name = value;
//                RaisePropertyChanged("Name");
//            }
//        }

//        public string Url
//        {
//            get
//            {
//                return _url;
//            }
//            set
//            {
//                _url = value;
//                RaisePropertyChanged("Url");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class Info : INotifyPropertyChanged
//    {
//        private string _text;

//        public string Text
//        {
//            get
//            {
//                return _text;
//            }
//            set
//            {
//                _text = value;
//                RaisePropertyChanged("Text");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class JSONCode : INotifyPropertyChanged
//    {
//        private string _code;

//        public string Code
//        {
//            get
//            {
//                return _code;
//            }
//            set
//            {
//                _code = value;
//                RaisePropertyChanged("Code");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }
//}
