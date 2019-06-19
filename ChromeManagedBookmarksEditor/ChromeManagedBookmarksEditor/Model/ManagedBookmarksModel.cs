using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using PropertyChanged;

namespace ChromeManagedBookmarksEditor.Model
{
    public class ManagedBookmarksModel { }

    [ImplementPropertyChanged]
    public class ManagedBookmarks
    {
        public string CurrentWorkingFolderPath { get; set; } = "";
        public string CurrentWorkingFolderContextMenuText { get; set; } = "";
        public Folder CurrentWorkingFolder { get; set; } = new Folder();
        public Folder RootFolder { get; set; } = new Folder();
    }

    [ImplementPropertyChanged]
    public class Folder
    {
        public Folder Parent { get; set; }
        public int FolderIndex { get; set; } = 0;
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

    public class URL : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                value = new string(value.Where(x => char.IsWhiteSpace(x) || char.IsLetterOrDigit(x)).ToArray());
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                char[] validSymbols = { '/', '.', ':' };
                value = new string(value.Where(x => char.IsLetterOrDigit(x) || validSymbols.Contains(x)).ToArray());
                _url = value;
                RaisePropertyChanged("Url");
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    [ImplementPropertyChanged]
    public class Info
    {
        public string LoadText { get; set; } = "Load";
        public string CopyText { get; set; } = "Copy";
        public string SerializeText { get; set; } = "Serialize";

        public Visibility SerializingAnimation { get; set; } = Visibility.Hidden;
    }

    [ImplementPropertyChanged]
    public class JSONCode
    {
        public string Code { get; set; } = "";
    }

    [ImplementPropertyChanged]
    public class Banners
    {
        public enum BannerAction { AddNewFolder, RemoveFolder, RenameFolder, ClearAllData, Alert}

        public bool BannerBackingVisible { get; set; } = false;

        public BannerAction ActiveAction { get; set; }

        public bool FolderBannerVisible { get; set; } = false;
        public string FolderBannerText { get; set; } = "";
        public string FolderBannerButtonText { get; set; } = "";

        public bool AlertBannerVisible { get; set; } = false;
        public string AlertBannerText { get; set; } = "";
        public string AlertBannerButtonText { get; set; } = "";

        public bool LoadingBannerVisible { get; set; } = false;
        public string LoadingBannerText { get; set; } = "THIS IS A TEST";

        public void ShowFolderBanner(string Message, string ConfirmButtonText, BannerAction Action)
        {
            ActiveAction = Action;
            FolderBannerText = Message;
            FolderBannerButtonText = ConfirmButtonText;
            BannerBackingVisible = true;
            FolderBannerVisible = true;
        }
        public void HideFolderBanner()
        {
            BannerBackingVisible = false;
            FolderBannerVisible = false;
        }

        public void ShowAlertBanner(string Message,string ConfirmButtonText, BannerAction Action)
        {
            ActiveAction = Action;
            AlertBannerText = Message;
            AlertBannerButtonText = ConfirmButtonText;
            BannerBackingVisible = true;
            AlertBannerVisible = true;
        }
        public void HideAlertBanner()
        {
            BannerBackingVisible = false;
            AlertBannerVisible = false;
        }

        public void ShowLoadingBanner(string Message)
        {
            LoadingBannerText = Message;
            BannerBackingVisible = true;
            LoadingBannerVisible = true;
        }
        public void HideLoadingBanner()
        {
            BannerBackingVisible = false;
            LoadingBannerVisible = false;
        }
    }
}
