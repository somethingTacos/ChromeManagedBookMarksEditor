using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Collections.ObjectModel;
using ChromeManagedBookmarksEditor.Model;
using ChromeManagedBookmarksEditor.Helpers;
using System.Linq;

namespace ChromeManagedBookmarksEditor.ViewModel
{
    public class ManagedBookmarksViewModel
    {
        #region Properties
        public ManagedBookmarks ChromeBookmarks { get; set; }

        public MyICommand SerializeCommand { get; set; }
        public MyICommand CopyCommand { get; set; }
        public MyICommand LoadCommand { get; set; }
        public MyICommand AddFolderCommand { get; set; }
        public MyICommand AddUrlCommand { get; set; }
        public MyICommand RemoveFolderCommand { get; set; }
        public MyICommand RemoveUrlCommand { get; set; }
        public MyICommand ClearAllCommand { get; set; }
        public MyICommand ShowHelpCommand { get; set; }
        public MyICommand ItemSelectedCommand { get; set; }

        public JSONCode Json { get; set; }
        public Info Info { get; set; }
        public bool _canLoad { get; set; }

        private NavigationViewModel _navigationViewModel { get; set; }
        #endregion

        #region Default Contructor
        public ManagedBookmarksViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            Json = (new JSONCode { Code = "Enter Chrome JSON Here" });
            Info = (new Info { Text = "Info: " });

            CopyCommand = new MyICommand(OnCopyCommand, CanCopyCommand);
            LoadCommand = new MyICommand(OnLoadCommand, CanLoadCommand);
            ClearAllCommand = new MyICommand(OnClearAllCommand, CanClearAllCommand);
            SerializeCommand = new MyICommand(OnSerializeCommand, CanSerializeCommand);
            RemoveUrlCommand = new MyICommand(OnRemoveUrlCommand, CanRemoveUrlCommand);
            RemoveFolderCommand = new MyICommand(OnRemoveFolderCommand, CanRemoveFolderCommand);
            AddUrlCommand = new MyICommand(OnAddUrlCommand, CanAddUrlCommand);
            AddFolderCommand = new MyICommand(OnAddFolderCommand, CanAddFolderCommand);
            ShowHelpCommand = new MyICommand(onShowHelpCommand, canShowHelpCommand);
            ItemSelectedCommand = new MyICommand(onItemSelectedCommand, canItemSelectedCommand);
            LoadTree();
            _canLoad = true;
        }
        #endregion

        #region Start Tasks
        private void LoadTree()
        {
            ManagedBookmarks tempMB = new ManagedBookmarks();

            //Dummy Data to help hookup views
            tempMB.CurrentWorkingFolder = "Dummy > Path > Test";

            tempMB.Folders.Add(new Folder { Name = "Folder 1" });
            tempMB.Folders.Add(new Folder { Name = "Folder 2" });

            tempMB.URLs.Add(new URL { Name = "URL 1", Url = "http://url1.com" });
            tempMB.URLs.Add(new URL { Name = "URL 2", Url = "http://url2.com" });

            ChromeBookmarks = tempMB;
        }
        #endregion

        #region Commands Code
        public void onItemSelectedCommand(object parameter)
        {
            ClearSelectedItems();

            if(parameter is Folder folder)
            {
                folder.IsSelected = true;
            }

            if(parameter is URL url)
            {
                url.IsSelected = true;
            }
        }
        public bool canItemSelectedCommand()
        {
            return true;
        }

        public void onShowHelpCommand(object parameter)
        {
            _navigationViewModel.SelectedViewModel = new HelpViewModel(_navigationViewModel);
        }
        public bool canShowHelpCommand()
        {
            return true;
        }

        private async void OnSerializeCommand(object parameter) //This method is going to be changed
        {
            string ConvertedCode = string.Empty;
            changeInfo("Serializeing Tree...");
            //ConvertedCode = await ConvertTreeToJSON(ChromeBookmarks);       
            Json.Code = ConvertedCode;
            changeInfo("Tree Serialized");
        }

        private bool CanSerializeCommand()
        {
            return true;
        }

        private void OnCopyCommand(object parameter)
        {
            try
            {
                Clipboard.SetText(Json.Code);
                changeInfo("Text Copied to clipboard");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                changeInfo("Error coping to clipboard");
            }
        }

        private bool CanCopyCommand()
        {
            return true;
        }


        private async void OnLoadCommand(object parameter) //This method is going to be changed
        {
            //try
            //{
            //    ObservableCollection<ManagedBookmarks> tempBookMarks = new ObservableCollection<ManagedBookmarks>();
            //    _canLoad = false;
            //    LoadCommand.RaiseCanExecuteChanged();
            //    changeInfo("Loading JSON...");
            //    //tempBookMarks = await LoadJSON(json.Code);

            //    if(tempBookMarks != null)
            //    {
            //        ChromeBookmarks.Clear();

            //        foreach (ManagedBookmarks bkmk in tempBookMarks)
            //        {
            //            ChromeBookmarks.Add(bkmk);
            //        }

            //        changeInfo("JSON Code loaded into TreeView");
            //    }
            //    else
            //    {
            //        changeInfo("Failed to load JSON, check syntax");
            //    }

            //    _canLoad = true;
            //    LoadCommand.RaiseCanExecuteChanged();
            //}
            //catch(Exception)
            //{
            //    changeInfo("Something went wrong, please restart program.  :(");
            //}
        }

        private bool CanLoadCommand()
        {
            return _canLoad;
        }
        #endregion

        #region Misc Methods
        private void changeInfo(string message)
        {
            Info.Text = String.Format("Info:  {0}", message);
        }

        private void ClearSelectedItems()
        {
            foreach(Folder folder in ChromeBookmarks.Folders)
            {
                folder.IsSelected = false;
            }
            foreach(URL url in ChromeBookmarks.URLs)
            {
                url.IsSelected = false;
            }
        }
        #endregion

        private object _selectedItem;

        //I hate this setup, I want to find a better way to do this.
        public object HighlightedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    AddUrlCommand.RaiseCanExecuteChanged();
                    AddFolderCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // ADD FOLDER -- this whole 'HighlightedItem' thing is going to be reworked.
        private void OnAddFolderCommand(object parameter)
        {
            if (HighlightedItem != null)
            {
                if (HighlightedItem is ManagedBookmarks)
                {
                    ManagedBookmarks root = (ManagedBookmarks)HighlightedItem;
                    root.Folders.Add(new Folder { Name = "", URLs = new ObservableCollection<URL>(), folders = new ObservableCollection<Folder>() });
                }

                if (HighlightedItem is Folder)
                {
                    Folder folder = (Folder)HighlightedItem;
                    folder.folders.Add(new Folder { Name = "", URLs = new ObservableCollection<URL>(), folders = new ObservableCollection<Folder>() });
                }
            }
        }

        private bool CanAddFolderCommand()
        {
            return HighlightedItem != null && HighlightedItem is Folder || HighlightedItem is ManagedBookmarks;
        }

        // ADD URL -- this whole 'HighlightedItem' thing is going to be reworked.
        private void OnAddUrlCommand(object parameter)
        {
            if(HighlightedItem != null)
            {
                if(HighlightedItem is ManagedBookmarks)
                {
                    ManagedBookmarks root = (ManagedBookmarks)HighlightedItem;
                    root.URLs.Add(new URL { Name = "", Url = "" });
                }

                if (HighlightedItem is Folder)
                {
                    Folder folder = (Folder)HighlightedItem;
                    folder.URLs.Add(new URL { Name = "", Url = "" });
                }
            }
        }

        private bool CanAddUrlCommand()
        {
            return HighlightedItem != null && HighlightedItem is Folder || HighlightedItem is ManagedBookmarks;
        }
        
        // REMOVE FOLDER -- redo this using linq
        private void OnRemoveFolderCommand(object parameter)
        {
            //if(HighlightedItem != null)
            //{
            //    if(HighlightedItem is Folder)
            //    {
            //        ChromeBookmarks[0].Folders.Remove((Folder)HighlightedItem);

            //        void removeFromSubFolders(Folder subFolder)
            //        {
            //            if (HighlightedItem is Folder)
            //            {
            //                subFolder.folders.Remove((Folder)HighlightedItem);

            //                if (subFolder.folders != null)
            //                {
            //                    foreach (Folder folder in subFolder.folders)
            //                    {
            //                        removeFromSubFolders(folder);
            //                    }
            //                }
            //            }
            //        }

            //        foreach (Folder folder in ChromeBookmarks[0].Folders)
            //        {
            //            if (HighlightedItem is Folder)
            //            {
            //                folder.folders.Remove((Folder)HighlightedItem);

            //                if (folder.folders != null)
            //                {
            //                    foreach (Folder subFolder in folder.folders)
            //                    {
            //                        removeFromSubFolders(subFolder);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private bool CanRemoveFolderCommand()
        {
            return HighlightedItem != null && HighlightedItem is Folder;
        }

        // REMOVE URL -- redo this using linq
        private void OnRemoveUrlCommand(object parameter)
        {
            //if (HighlightedItem != null)
            //{
            //    if (HighlightedItem is URL)
            //    {
            //        ChromeBookmarks[0].URLs.Remove((URL)HighlightedItem);

            //        void removeFromSubFolders(Folder subFolder)
            //        {
            //            if (HighlightedItem is URL)
            //            {
            //                subFolder.URLs.Remove((URL)HighlightedItem);

            //                if (subFolder.folders != null)
            //                {
            //                    foreach (Folder folder in subFolder.folders)
            //                    {
            //                        removeFromSubFolders(folder);
            //                    }
            //                }
            //            }
            //        }

            //        foreach (Folder folder in ChromeBookmarks[0].Folders)
            //        {
            //            if (HighlightedItem is URL)
            //            {
            //                folder.URLs.Remove((URL)HighlightedItem);

            //                if (folder.folders != null)
            //                {
            //                    foreach (Folder subFolder in folder.folders)
            //                    {
            //                        removeFromSubFolders(subFolder);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private bool CanRemoveUrlCommand()
        {
            return HighlightedItem != null && HighlightedItem is URL;
        }

        // CLEAR ALL -- Not sure what I'm going to do for this yet, probably just a button or something, idk
        private void OnClearAllCommand(object parameter)
        {
            //ChromeBookmarks.Clear();
            //ChromeBookmarks.Add(new ManagedBookmarks { toplevel_name = "Root Folder", URLs = new ObservableCollection<URL>(), Folders = new ObservableCollection<Folder>() });
        }

        private bool CanClearAllCommand()
        {
            return true;
            //return (ChromeBookmarks[0].Folders != null || ChromeBookmarks[0].URLs != null);
        }
    }
}
