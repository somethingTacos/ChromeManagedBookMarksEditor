using System.Collections;
using System.Collections.ObjectModel;
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
        //public string toplevel_name { get; set; } = "Root Folder";  //maybe just make this another folder... or not... idk..., I might not need this...

        //Might not need these anymore... idk...
        //public ObservableCollection<Folder> Folders { get; set; } = new ObservableCollection<Folder>();
        //public ObservableCollection<URL> URLs { get; set; } = new ObservableCollection<URL>();
        //public IList Children
        //{
        //    get
        //    {
        //        return new CompositeCollection()
        //        {
        //            new CollectionContainer() { Collection = Folders },
        //            new CollectionContainer() { Collection = URLs }
        //        };
        //    }
        //}
    }

    //[ImplementPropertyChanged]
    //public class RootFolder
    //{
    //    public string toplevel_name { get; set; } = "";
    //}

    //[ImplementPropertyChanged]
    //public class ParsedFolders
    //{
    //    public string Name { get; set; } = "";
    //    public JArray children { get; set; } = new JArray();
    //}

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
        public string LoadText { get; set; } = "Load";
        public string CopyText { get; set; } = "Copy";
        public string SerializeText { get; set; } = "Serialize";

        public Visibility SerializingAnimation { get; set; } = Visibility.Hidden;
        public Visibility LoadingAnimation { get; set; } = Visibility.Hidden;
    }

    [ImplementPropertyChanged]
    public class JSONCode
    {
        public string Code { get; set; } = "";
    }

    [ImplementPropertyChanged]
    public class BannerInfo //I think this might need a rename... it's a bit more than just informative at this point...
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
    }
}
