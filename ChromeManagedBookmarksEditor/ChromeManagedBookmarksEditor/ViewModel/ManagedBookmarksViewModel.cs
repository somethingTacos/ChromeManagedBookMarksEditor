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

        public BannerInfo Banners { get; set; }

        public MyICommand SerializeCommand { get; set; }
        public MyICommand CopyCommand { get; set; }
        public MyICommand LoadCommand { get; set; }
        public MyICommand StartAddFolderCommand { get; set; }
        public MyICommand AddNewFolderCommand { get; set; }
        public MyICommand CancelAddNewFolderCommand { get; set; }
        public MyICommand AddUrlCommand { get; set; }
        public MyICommand RemoveSelectedCommand { get; set; }
        public MyICommand ConfirmBulkRemoveCommand { get; set; }
        public MyICommand CancelBulkRemoveCommand { get; set; }
        public MyICommand EnterFolderCommand { get; set; }
        public MyICommand ExitFolderCommand { get; set; }
        public MyICommand ClearAllCommand { get; set; }
        public MyICommand ShowHelpCommand { get; set; }
        public MyICommand ItemSelectedCommand { get; set; }

        public Folder NewFolder { get; set; }
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
            Info = new Info();

            CopyCommand = new MyICommand(OnCopyCommand, CanCopyCommand);
            LoadCommand = new MyICommand(OnLoadCommand, CanLoadCommand);
            ClearAllCommand = new MyICommand(OnClearAllCommand, CanClearAllCommand);
            SerializeCommand = new MyICommand(OnSerializeCommand, CanSerializeCommand);
            RemoveSelectedCommand = new MyICommand(onRemoveSelectedCommand, canRemoveSelectedCommand);
            AddUrlCommand = new MyICommand(onAddUrlCommand, CanAddUrlCommand);
            StartAddFolderCommand = new MyICommand(OnStartAddFolderCommand, canStartAddFolderCommand);
            AddNewFolderCommand = new MyICommand(onAddNewFolderCommand, canAddNewFolderCommand);
            ShowHelpCommand = new MyICommand(onShowHelpCommand, canShowHelpCommand);
            ItemSelectedCommand = new MyICommand(onItemSelectedCommand, canItemSelectedCommand);
            ConfirmBulkRemoveCommand = new MyICommand(onConfirmBulkRemoveCommand, canConfirmBulkRemoveCommand);
            CancelBulkRemoveCommand = new MyICommand(onCancelBulkRemoveCommand, canCancelBulkRemoveCommand);
            CancelAddNewFolderCommand = new MyICommand(onCancelAddNewFolderCommand, canCancelAddNewFolderCommand);
            EnterFolderCommand = new MyICommand(onEnterFolderCommand, canEnterFolderCommand);
            ExitFolderCommand = new MyICommand(onExitFolderCommand, canExitFolderCommand);
            LoadTree();
            _canLoad = true;
        }
        #endregion

        #region Start Tasks
        private void LoadTree()
        {
            Folder tempFolder = new Folder();
            ManagedBookmarks tempMB = new ManagedBookmarks();
            BannerInfo tempBI = new BannerInfo();

            tempMB.CurrentWorkingFolder.Name = "Root Folder";
            tempMB.CurrentWorkingFolderPath = "Root Folder";

            ChromeBookmarks = tempMB;
            Banners = tempBI;
            NewFolder = tempFolder;
        }
        #endregion

        #region Commands Code
        private void onCancelAddNewFolderCommand(object parameter)
        {
            NewFolder.Name = "";
            Banners.HideNewFolderBanner();
        }
        private bool canCancelAddNewFolderCommand()
        {
            return true;
        }

        private void onAddNewFolderCommand(object parameter)
        {
            if(parameter is Folder parentFolder)
            {
                Folder newFolder = new Folder();
                newFolder.Name = NewFolder.Name.ToString();
                newFolder.Parent = parentFolder;
                newFolder.FolderIndex = parentFolder.FolderIndex + 1;

                parentFolder.folders.Add(newFolder);
            }

            NewFolder.Name = "";
            Banners.HideNewFolderBanner();
        }
        private bool canAddNewFolderCommand()
        {
            //check if the folder name already exists inside the current folder.
            return true;
        }
        private void onEnterFolderCommand(object parameter)
        {
            if (parameter is Folder subFolder)
            {
                ChromeBookmarks.CurrentWorkingFolder = subFolder;
                UpdateWorkingPath();
            }
        }
        private bool canEnterFolderCommand()
        {
            return ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected == true).Count() > 0;
        }

        public void onExitFolderCommand(object parameter)
        {
            ChromeBookmarks.CurrentWorkingFolder = ChromeBookmarks.CurrentWorkingFolder.Parent;
            UpdateWorkingPath();
        }
        public bool canExitFolderCommand()
        {
            //need to make sure folder is not root folder.
            return ChromeBookmarks.CurrentWorkingFolder.FolderIndex != 0;
        }

        public void onItemSelectedCommand(object parameter)
        {
            ClearSelectedItems();

            if (parameter is Folder folder)
            {
                folder.IsSelected = true;
            }

            if (parameter is URL url)
            {
                url.IsSelected = true;
            }

            EnterFolderCommand.RaiseCanExecuteChanged();
            RemoveSelectedCommand.RaiseCanExecuteChanged();
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
            //string ConvertedCode = string.Empty;
            //changeInfo("Serializeing Tree...");
            ////ConvertedCode = await ConvertTreeToJSON(ChromeBookmarks);
            //Json.Code = ConvertedCode;
            //changeInfo("Tree Serialized");
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
                Info.CopyText = "Copied!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Info.CopyText = "Error :(";
                MessageBox.Show(ex.Message);
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

        private void OnStartAddFolderCommand(object parameter)
        {
            Banners.ShowNewFolderBanner();
        }
        private bool canStartAddFolderCommand()
        {
            return true;
        }

        private void onAddUrlCommand(object parameter)
        {
            ChromeBookmarks.CurrentWorkingFolder.URLs.Add(new URL { Name = "", Url = "" });
        }
        private bool CanAddUrlCommand()
        {
            return true; // ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected == true).Count() > 0;
        }

        private void onRemoveSelectedCommand(object parameter)
        {
            if(ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).Count() > 0)
            {
                Folder SelectedFolder = ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).FirstOrDefault();
                string AlertMessage = "";

                if (SelectedFolder.folders.Count > 0 || SelectedFolder.URLs.Count > 0)
                {
                    AlertMessage += $"Folder '{SelectedFolder.Name}' Contains ";
                    if(SelectedFolder.folders.Count() > 0)
                    {
                        if(SelectedFolder.folders.Count > 1)
                        {
                            AlertMessage += $"{SelectedFolder.folders.Count()} folders";
                        }
                        else
                        {
                            AlertMessage += $"{SelectedFolder.folders.Count()} folder";
                        }

                        if (SelectedFolder.URLs.Count() > 0)
                        {
                            AlertMessage += " and ";
                        }
                    }

                    if(SelectedFolder.URLs.Count() > 0)
                    {
                        if(SelectedFolder.URLs.Count() > 1)
                        {
                             AlertMessage += $"{SelectedFolder.URLs.Count()} URLs";
                        }
                        else
                        {
                            AlertMessage += $"{SelectedFolder.URLs.Count()} URL";
                        }
                    }

                    Banners.ShowAlertBanner(AlertMessage);
                }
                else
                {
                    ChromeBookmarks.CurrentWorkingFolder.folders.Remove(ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected == true).FirstOrDefault());
                }
            }
            if (ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected).Count() > 0)
            {
                ChromeBookmarks.CurrentWorkingFolder.URLs.Remove(ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected == true).FirstOrDefault());
            }

            RemoveSelectedCommand.RaiseCanExecuteChanged();
        }
        private bool canRemoveSelectedCommand()
        {
            return ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected == true).Count() > 0 || ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected == true).Count() > 0;
        }

        private void onConfirmBulkRemoveCommand(object parameter)
        {
            ChromeBookmarks.CurrentWorkingFolder.folders.Remove(ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected == true).FirstOrDefault());
            ClearSelectedItems();
            Banners.HideAlertBanner();
        }
        private bool canConfirmBulkRemoveCommand()
        {
            return true;
        }

        private void onCancelBulkRemoveCommand(object parameter)
        {
            Banners.HideAlertBanner();
        }
        private bool canCancelBulkRemoveCommand()
        {
            return true;
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
        }
        #endregion

        #region Misc Methods

        private void ClearSelectedItems()
        {
            foreach (Folder folder in ChromeBookmarks.CurrentWorkingFolder.folders)
            {
                folder.IsSelected = false;
            }
            foreach (URL url in ChromeBookmarks.CurrentWorkingFolder.URLs)
            {
                url.IsSelected = false;
            }

            RemoveSelectedCommand.RaiseCanExecuteChanged();
        }

        private void UpdateWorkingPath()
        {
            string newPath = string.Empty;
            List<string> reversePathList = new List<string>();
            int index = ChromeBookmarks.CurrentWorkingFolder.FolderIndex;
            Folder parentFolder = ChromeBookmarks.CurrentWorkingFolder;

            reversePathList.Add(parentFolder.Name.ToString());

            for(int i = 0; i < index; i++)
            {
                reversePathList.Add(parentFolder.Parent.Name.ToString());
                parentFolder = parentFolder.Parent;
            }

            for(int i = reversePathList.Count() -1; i >= 0; i--)
            {
                newPath += reversePathList[i].ToString();

                if(i != 0)
                {
                    newPath += " > ";
                }
            }

            ChromeBookmarks.CurrentWorkingFolderPath = newPath;
            ClearSelectedItems();
            ExitFolderCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
