using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Collections.ObjectModel;
using LinkHierarchy2JSON.Model;
using LinkHierarchy2JSON.Helpers;

namespace LinkHierarchy2JSON.ViewModel
{
    public class ManagedBookmarksViewModel
    {
        //Need to add regions to make viewing easier
        public ObservableCollection<ManagedBookmarks> ChromeBookmarks { get; set; }
        public MyICommand SerializeCommand { get; set; }
        public MyICommand CopyCommand { get; set; }
        public MyICommand LoadCommand { get; set; }
        public MyICommand AddFolderCommand { get; set; }
        public MyICommand AddUrlCommand { get; set; }
        public MyICommand RemoveFolderCommand { get; set; }
        public MyICommand RemoveUrlCommand { get; set; }
        public MyICommand ClearAllCommand { get; set; }
        public MyICommand ShowHelpCommand { get; set; }
        public JSONCode json { get; set; }
        public Info info { get; set; }
        public bool _canLoad { get; set; }

        private NavigationViewModel _navigationViewModel { get; set; }

        public ManagedBookmarksViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            json = (new JSONCode { Code = "Enter Chrome JSON Here" });
            info = (new Info { Text = "Info: " });

            CopyCommand = new MyICommand(OnCopyCommand, CanCopyCommand);
            LoadCommand = new MyICommand(OnLoadCommand, CanLoadCommand);
            ClearAllCommand = new MyICommand(OnClearAllCommand, CanClearAllCommand);
            SerializeCommand = new MyICommand(OnSerializeCommand, CanSerializeCommand);
            RemoveUrlCommand = new MyICommand(OnRemoveUrlCommand, CanRemoveUrlCommand);
            RemoveFolderCommand = new MyICommand(OnRemoveFolderCommand, CanRemoveFolderCommand);
            AddUrlCommand = new MyICommand(OnAddUrlCommand, CanAddUrlCommand);
            AddFolderCommand = new MyICommand(OnAddFolderCommand, CanAddFolderCommand);
            ShowHelpCommand = new MyICommand(onShowHelpCommand, canShowHelpCommand);
            LoadTree();
            _canLoad = true;
        }

        public void onShowHelpCommand()
        {
            _navigationViewModel.SelectedViewModel = new HelpViewModel(_navigationViewModel);
        }
        public bool canShowHelpCommand()
        {
            return true;
        }

        private async void OnSerializeCommand() //This method is going to be changed
        {
            string ConvertedCode = string.Empty;
            changeInfo("Serializeing Tree...");
            //ConvertedCode = await ConvertTreeToJSON(ChromeBookmarks);       
            json.Code = ConvertedCode;
            changeInfo("Tree Serialized");
        }

        private bool CanSerializeCommand()
        {
            return true;
        }

        
        private void LoadTree()
        {
            ObservableCollection<ManagedBookmarks> tempOC = new ObservableCollection<ManagedBookmarks>();
            tempOC.Add(new ManagedBookmarks());

            ChromeBookmarks = tempOC;
        }


        private void changeInfo(string message)
        {
            info.Text = String.Format("Info:  {0}", message);
        }

        private void OnCopyCommand()
        {
            try
            {
                Clipboard.SetText(json.Code);
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


        private async void OnLoadCommand() //This method is going to be changed
        {
            try
            {
                ObservableCollection<ManagedBookmarks> tempBookMarks = new ObservableCollection<ManagedBookmarks>();
                _canLoad = false;
                LoadCommand.RaiseCanExecuteChanged();
                changeInfo("Loading JSON...");
                //tempBookMarks = await LoadJSON(json.Code);

                if(tempBookMarks != null)
                {
                    ChromeBookmarks.Clear();

                    foreach (ManagedBookmarks bkmk in tempBookMarks)
                    {
                        ChromeBookmarks.Add(bkmk);
                    }

                    changeInfo("JSON Code loaded into TreeView");
                }
                else
                {
                    changeInfo("Failed to load JSON, check syntax");
                }

                _canLoad = true;
                LoadCommand.RaiseCanExecuteChanged();
            }
            catch(Exception)
            {
                changeInfo("Something went wrong, please restart program.  :(");
            }
        }

        private bool CanLoadCommand()
        {
            return _canLoad;
        }

        

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
        private void OnAddFolderCommand()
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
        private void OnAddUrlCommand()
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
        private void OnRemoveFolderCommand()
        {
            if(HighlightedItem != null)
            {
                if(HighlightedItem is Folder)
                {
                    ChromeBookmarks[0].Folders.Remove((Folder)HighlightedItem);

                    void removeFromSubFolders(Folder subFolder)
                    {
                        if (HighlightedItem is Folder)
                        {
                            subFolder.folders.Remove((Folder)HighlightedItem);

                            if (subFolder.folders != null)
                            {
                                foreach (Folder folder in subFolder.folders)
                                {
                                    removeFromSubFolders(folder);
                                }
                            }
                        }
                    }

                    foreach (Folder folder in ChromeBookmarks[0].Folders)
                    {
                        if (HighlightedItem is Folder)
                        {
                            folder.folders.Remove((Folder)HighlightedItem);

                            if (folder.folders != null)
                            {
                                foreach (Folder subFolder in folder.folders)
                                {
                                    removeFromSubFolders(subFolder);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CanRemoveFolderCommand()
        {
            return HighlightedItem != null && HighlightedItem is Folder;
        }

        // REMOVE URL -- redo this using linq
        private void OnRemoveUrlCommand()
        {
            if (HighlightedItem != null)
            {
                if (HighlightedItem is URL)
                {
                    ChromeBookmarks[0].URLs.Remove((URL)HighlightedItem);

                    void removeFromSubFolders(Folder subFolder)
                    {
                        if (HighlightedItem is URL)
                        {
                            subFolder.URLs.Remove((URL)HighlightedItem);

                            if (subFolder.folders != null)
                            {
                                foreach (Folder folder in subFolder.folders)
                                {
                                    removeFromSubFolders(folder);
                                }
                            }
                        }
                    }

                    foreach (Folder folder in ChromeBookmarks[0].Folders)
                    {
                        if (HighlightedItem is URL)
                        {
                            folder.URLs.Remove((URL)HighlightedItem);

                            if (folder.folders != null)
                            {
                                foreach (Folder subFolder in folder.folders)
                                {
                                    removeFromSubFolders(subFolder);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CanRemoveUrlCommand()
        {
            return HighlightedItem != null && HighlightedItem is URL;
        }

        // CLEAR ALL -- Not sure what I'm going to do for this yet, probably just a button or something, idk
        private void OnClearAllCommand()
        {
            ChromeBookmarks.Clear();
            ChromeBookmarks.Add(new ManagedBookmarks { toplevel_name = "Root Folder", URLs = new ObservableCollection<URL>(), Folders = new ObservableCollection<Folder>() });
        }

        private bool CanClearAllCommand()
        {
            return (ChromeBookmarks[0].Folders != null || ChromeBookmarks[0].URLs != null);
        }
    }
}
