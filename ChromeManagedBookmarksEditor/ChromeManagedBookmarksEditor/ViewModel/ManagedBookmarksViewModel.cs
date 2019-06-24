using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using ChromeManagedBookmarksEditor.Model;
using ChromeManagedBookmarksEditor.Helpers;
using System.Linq;
using System.Windows.Threading;

namespace ChromeManagedBookmarksEditor.ViewModel
{
    public class ManagedBookmarksViewModel
    {
        #region Properties
        public MyICommand ConfirmFolderBannerCommand { get; set; }
        public MyICommand CancelFolderBannerCommand { get; set; }
        public MyICommand ConfirmAlertBannerCommand { get; set; }
        public MyICommand CancelAlertBannerCommand { get; set; }
        public MyICommand RemoveSelectedCommand { get; set; }
        public MyICommand AddNewFolderCommand { get; set; }
        public MyICommand RenameSelectedFolderCommand { get; set; }
        public MyICommand RenameParentFolderCommand { get; set; }
        public MyICommand ItemSelectedCommand { get; set; }
        public MyICommand EnterFolderCommand { get; set; }
        public MyICommand ExitFolderCommand { get; set; }
        public MyICommand SerializeCommand { get; set; }
        public MyICommand ShowHelpCommand { get; set; }
        public MyICommand ClearAllCommand { get; set; }
        public MyICommand AddUrlCommand { get; set; }
        public MyICommand LoadCommand { get; set; }
        public MyICommand CopyCommand { get; set; }

        private NavigationViewModel _navigationViewModel { get; set; }
        public static ManagedBookmarks ChromeBookmarks { get; set; }
        public BannerData Banners { get; set; }
        public Folder NewFolder { get; set; }
        public bool _canLoad { get; set; }
        public JSONCode Json { get; set; }
        public Info Info { get; set; }

        public DispatcherTimer CopyTimer = new DispatcherTimer();
        public DispatcherTimer LoadTimer = new DispatcherTimer();

        #endregion

        #region Default Contructor
        public ManagedBookmarksViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            Json = (new JSONCode { Code = "Enter Chrome JSON Here" });
            Info = new Info();
            //Misc Commands
            CopyCommand = new MyICommand(OnCopyCommand, CanCopyCommand);
            LoadCommand = new MyICommand(OnLoadCommand, CanLoadCommand);
            ClearAllCommand = new MyICommand(OnClearAllCommand, CanClearAllCommand);
            SerializeCommand = new MyICommand(OnSerializeCommand, CanSerializeCommand);
            ShowHelpCommand = new MyICommand(onShowHelpCommand, canShowHelpCommand);

            //Alert Banner Commands
            ConfirmAlertBannerCommand = new MyICommand(onConfirmAlertBannerCommand, canConfirmAlertBannerCommand);
            CancelAlertBannerCommand = new MyICommand(onCancelAlertBannerCommand, canCancelAlertBannerCommand);
            //Folder Banner Commands
            ConfirmFolderBannerCommand = new MyICommand(OnConfirmFolderBannerCommand, canConfirmFolderBannerCommand);
            CancelFolderBannerCommand = new MyICommand(onCancelFolderBannerCommand, canCancelFolderBannerCommand);
            //Item Commands
            AddNewFolderCommand = new MyICommand(onAddFolderCommand, canAddFolderCommand);
            RenameSelectedFolderCommand = new MyICommand(onRenameSelectedFolderCommand, canRenameSelectedFolderCommand);
            RenameParentFolderCommand = new MyICommand(onRenameParentFolderCommand, canRenameParentFolderCommand);
            AddUrlCommand = new MyICommand(onAddUrlCommand, CanAddUrlCommand);
            RemoveSelectedCommand = new MyICommand(onRemoveSelectedCommand, canRemoveSelectedCommand);
            ItemSelectedCommand = new MyICommand(onItemSelectedCommand, canItemSelectedCommand);
            //Folder access commands
            EnterFolderCommand = new MyICommand(onEnterFolderCommand, canEnterFolderCommand);
            ExitFolderCommand = new MyICommand(onExitFolderCommand, canExitFolderCommand);

            CopyTimer.Interval = TimeSpan.FromSeconds(2);
            CopyTimer.Tick += CopyTimer_Tick;

            LoadTimer.Interval = TimeSpan.FromSeconds(1);
            LoadTimer.Tick += LoadTimer_Tick;

            LoadTree();
            _canLoad = true;
        }
        #endregion

        #region Start Tasks
        private void LoadTree()
        {
            Folder tempFolder = new Folder();
            BannerData tempBI = new BannerData();

            if (ChromeBookmarks == null)
            {
                ManagedBookmarks tempMB = new ManagedBookmarks();
                tempMB.RootFolder.Name = "Root Folder";

                tempMB.CurrentWorkingFolder = tempMB.RootFolder;

                ChromeBookmarks = tempMB;
            }

            Banners = tempBI;
            NewFolder = tempFolder;

            UpdateWorkingPath();
        }
        #endregion

        #region Commands Code
        private void onRenameParentFolderCommand(object parameter)
        {
            if (parameter is Folder folderToRename)
            {
                ClearSelectedItems();
                Banners.ShowFolderBanner($"Enter a new name for '{folderToRename.Name}'", "Rename", BannerData.BannerAction.RenameFolder);
            }
        }
        private bool canRenameParentFolderCommand()
        {
            return true;
        }
        private void onRenameSelectedFolderCommand(object parameter)
        {
            if(parameter is Folder folderToRename)
            {
                Banners.ShowFolderBanner($"Enter a new name for '{folderToRename.Name}'", "Rename", BannerData.BannerAction.RenameFolder);
            }
        }
        private bool canRenameSelectedFolderCommand()
        {
            return true;
        }
        private void onCancelFolderBannerCommand(object parameter)
        {
            switch (Banners.ActiveAction)
            {
                case BannerData.BannerAction.AddNewFolder:
                    {
                        NewFolder.Name = "";
                        Banners.HideFolderBanner();
                        break;
                    }
                case BannerData.BannerAction.RenameFolder:
                    {
                        NewFolder.Name = "";
                        Banners.HideFolderBanner();
                        break;
                    }
            }
        }
        private bool canCancelFolderBannerCommand()
        {
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
            return ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).Count() > 0;
        }

        public void onExitFolderCommand(object parameter)
        {
            ChromeBookmarks.CurrentWorkingFolder = ChromeBookmarks.CurrentWorkingFolder.Parent;
            UpdateWorkingPath();
        }
        public bool canExitFolderCommand()
        {
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
            if (Info.SerializingAnimation != Visibility.Visible)
            {
                string ConvertedCode = string.Empty;
                Info.SerializingAnimation = Visibility.Visible;
                LoadCommand.RaiseCanExecuteChanged();
                Info.SerializeText = "Serializing...";
                ConvertedCode = await ChromeJSONConverter.ConvertToJSON(ChromeBookmarks.RootFolder);
                Json.Code = ConvertedCode;
                Info.SerializeText = "Serialize";
                Info.SerializingAnimation = Visibility.Hidden;
                LoadCommand.RaiseCanExecuteChanged();
            }
            else
            {
                Info.SerializeText = "Please Wait...";
            }
        }

        private bool CanSerializeCommand()
        {
            return !Banners.LoadingBannerVisible;
        }

        private void OnCopyCommand(object parameter)
        {
            try
            {
                Clipboard.SetText(Json.Code);
                Info.CopyText = "Copied!";
                CopyTimer.Start();
            }
            catch (Exception ex)
            {
                Info.CopyText = "Error :(";
                MessageBox.Show($"{ex.Message}\n\nIs another program locking the clipboard?", "Could not get clipboard data",MessageBoxButton.OK, MessageBoxImage.Error);
                Info.CopyText = "Copy";
            }
        }
        private bool CanCopyCommand()
        {
            return true;
        }

        private async void OnLoadCommand(object parameter) //This method is going to be changed
        {
            if (!Banners.LoadingBannerVisible)
            {
                ManagedBookmarks ParsedBookmarks = new ManagedBookmarks();
                Banners.ShowLoadingBanner("Loading JSON, please wait...");
                SerializeCommand.RaiseCanExecuteChanged();
                Info.LoadText = "Loading JSON...";
                ParsedBookmarks = await ChromeJSONConverter.ParseJSON(Json.Code);
                Info.LoadText = "Load";
                Banners.LoadingBannerText = "JSON Loaded";
                LoadTimer.Start();
                
                

                if (ParsedBookmarks.RootFolder.Name != "")
                {
                    ChromeBookmarks.RootFolder = ParsedBookmarks.RootFolder;
                    ChromeBookmarks.CurrentWorkingFolder = ParsedBookmarks.RootFolder;
                    ChromeBookmarks.CurrentWorkingFolderContextMenuText = $"Rename '{ParsedBookmarks.RootFolder.Name}'";
                    ChromeBookmarks.CurrentWorkingFolderPath = ParsedBookmarks.RootFolder.Name;
                }
            }
            else
            {
                Info.LoadText = "Please Wait...";
            }
        }

        private bool CanLoadCommand()
        {
            return Info.SerializingAnimation != Visibility.Visible;
        }

        private void onAddFolderCommand(object parameter)
        {
            Banners.ShowFolderBanner("New Folder Name", "Add Folder", BannerData.BannerAction.AddNewFolder);
        }
        private bool canAddFolderCommand()
        {
            return true;
        }

        private void OnConfirmFolderBannerCommand(object parameter)
        {
            switch(Banners.ActiveAction)
            {
                case BannerData.BannerAction.AddNewFolder:
                    {
                        if (NewFolder.Name != "")
                        {
                            NewFolder.Name = new string(NewFolder.Name.Where(x => char.IsWhiteSpace(x) || char.IsLetterOrDigit(x)).ToArray());

                            if (ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.Name == NewFolder.Name).Count() > 0)
                            {
                                Banners.HideFolderBanner();
                                Banners.ShowAlertBanner($"The name '{NewFolder.Name}' is already in use", "OK", BannerData.BannerAction.Alert);
                            }
                            else
                            {
                                if (parameter is Folder parentFolder)
                                {
                                    Folder newFolder = new Folder();
                                    newFolder.Name = NewFolder.Name.ToString().Trim();
                                    newFolder.Parent = parentFolder;
                                    newFolder.FolderIndex = parentFolder.FolderIndex + 1;

                                    parentFolder.folders.Add(newFolder);
                                    Banners.HideFolderBanner();
                                }
                            }
                        }
                        else
                        {
                            Banners.HideFolderBanner();
                            Banners.ShowAlertBanner($"The folder name cannot be blank", "OK", BannerData.BannerAction.Alert);
                        }

                        NewFolder.Name = "";
                        break;
                    }
                case BannerData.BannerAction.RenameFolder:
                    {
                        Folder FolderToRename = new Folder();
                        bool IsCurrentWorkingFolder = false;
                        
                        if(ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).Count() > 0)
                        {
                           FolderToRename = ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).FirstOrDefault();
                        }
                        else
                        {
                            FolderToRename = ChromeBookmarks.CurrentWorkingFolder;
                            IsCurrentWorkingFolder = true;
                        }

                        if (NewFolder.Name != "")
                        {
                            NewFolder.Name = new string(NewFolder.Name.Where(x => char.IsWhiteSpace(x) || char.IsLetterOrDigit(x)).ToArray());

                            if (IsCurrentWorkingFolder)
                            {
                                if (ChromeBookmarks.CurrentWorkingFolder.FolderIndex == 0 ? false : ChromeBookmarks.CurrentWorkingFolder.Parent.folders.Where(x => x.Name == NewFolder.Name).Count() > 0)
                                {
                                    Banners.HideFolderBanner();
                                    Banners.ShowAlertBanner($"The name '{NewFolder.Name}' is already in use", "OK", BannerData.BannerAction.Alert);
                                }
                                else
                                {
                                    FolderToRename.Name = NewFolder.Name.ToString().Trim();
                                    Banners.HideFolderBanner();
                                }
                            }
                            else
                            {
                                if (ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.Name == NewFolder.Name).Count() > 0)
                                {
                                    Banners.HideFolderBanner();
                                    Banners.ShowAlertBanner($"The name '{NewFolder.Name}' is already in use", "OK", BannerData.BannerAction.Alert);
                                }
                                else
                                {
                                    FolderToRename.Name = NewFolder.Name.ToString().Trim();
                                    Banners.HideFolderBanner();
                                }
                            }
                        }
                        else
                        {
                            Banners.HideFolderBanner();
                            Banners.ShowAlertBanner($"The folder name cannot be blank", "OK", BannerData.BannerAction.Alert);
                        }

                        NewFolder.Name = "";
                        UpdateWorkingPath();
                        break;
                    }
            }
        }
        private bool canConfirmFolderBannerCommand()
        {
            return true;
        }

        private void onAddUrlCommand(object parameter)
        {
            ChromeBookmarks.CurrentWorkingFolder.URLs.Add(new URL { Name = "", Url = "" });
        }
        private bool CanAddUrlCommand()
        {
            return true;
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

                    Banners.ShowAlertBanner(AlertMessage, "Remove", BannerData.BannerAction.RemoveFolder);
                }
                else
                {
                    ChromeBookmarks.CurrentWorkingFolder.folders.Remove(ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).FirstOrDefault());
                }
            }
            if (ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected).Count() > 0)
            {
                ChromeBookmarks.CurrentWorkingFolder.URLs.Remove(ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected).FirstOrDefault());
            }

            RemoveSelectedCommand.RaiseCanExecuteChanged();
        }
        private bool canRemoveSelectedCommand()
        {
            return ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).Count() > 0 || ChromeBookmarks.CurrentWorkingFolder.URLs.Where(x => x.IsSelected).Count() > 0;
        }

        private void onConfirmAlertBannerCommand(object parameter)
        {
            switch(Banners.ActiveAction)
            {
                case BannerData.BannerAction.RemoveFolder:
                    {
                        ChromeBookmarks.CurrentWorkingFolder.folders.Remove(ChromeBookmarks.CurrentWorkingFolder.folders.Where(x => x.IsSelected).FirstOrDefault());
                        ClearSelectedItems();
                        Banners.HideAlertBanner();
                        break;
                    }
                case BannerData.BannerAction.Alert:
                    {
                        Banners.HideAlertBanner();
                        break;
                    }
                case BannerData.BannerAction.ClearAllData:
                    {
                        ChromeBookmarks.RootFolder.Name = "Root Folder";
                        ChromeBookmarks.CurrentWorkingFolderPath = "Root Folder";
                        ChromeBookmarks.CurrentWorkingFolderContextMenuText = "Rename 'Root Folder'";
                        ChromeBookmarks.CurrentWorkingFolder = ChromeBookmarks.RootFolder;
                        ChromeBookmarks.RootFolder.folders.Clear();
                        ChromeBookmarks.RootFolder.URLs.Clear();
                        Banners.HideAlertBanner();
                        break;
                    }
            }
        }
        private bool canConfirmAlertBannerCommand()
        {
            return true;
        }

        private void onCancelAlertBannerCommand(object parameter)
        {
            Banners.HideAlertBanner();
        }
        private bool canCancelAlertBannerCommand()
        {
            return true;
        }

        private void OnClearAllCommand(object parameter)
        {
            Banners.ShowAlertBanner("Are you sure you want to clear all currently loaded managed bookmark data", "Clear Data", BannerData.BannerAction.ClearAllData);
        }

        private bool CanClearAllCommand()
        {
            return ChromeBookmarks.CurrentWorkingFolder == ChromeBookmarks.RootFolder;
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

            ChromeBookmarks.CurrentWorkingFolderContextMenuText = $"Rename '{ChromeBookmarks.CurrentWorkingFolder.Name}'";
            ChromeBookmarks.CurrentWorkingFolderPath = newPath;
            ClearSelectedItems();
            ExitFolderCommand.RaiseCanExecuteChanged();
            ClearAllCommand.RaiseCanExecuteChanged();
        }

        private void CopyTimer_Tick(object sender, EventArgs e)
        {
            CopyTimer.Stop();
            Info.CopyText = "Copy";
        }

        private void LoadTimer_Tick(object sender, EventArgs e)
        {
            LoadTimer.Stop();
            Banners.HideLoadingBanner();
            SerializeCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
