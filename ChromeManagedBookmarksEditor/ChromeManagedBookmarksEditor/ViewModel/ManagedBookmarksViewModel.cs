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

        private void OnAddFolderCommand(object parameter)
        {

        }
        private bool canAddFolderCommand()
        {
            return ChromeBookmarks.Folders.Where(x => x.IsSelected == true).Count() > 0;
        }

        private void onAddUrlCommand(object parameter)
        {

        }
        private bool CanAddUrlCommand()
        {
            return ChromeBookmarks.URLs.Where(x => x.IsSelected == true).Count() > 0;
        }

        // REMOVE FOLDER -- redo this using linq
        private void OnRemoveFolderCommand(object parameter)
        {

        }

        private bool CanRemoveFolderCommand()
        {
            return ChromeBookmarks.Folders.Where(x => x.IsSelected == true).Count() > 0;
        }

        // REMOVE URL -- redo this using linq
        private void OnRemoveUrlCommand(object parameter)
        {

        }

        private bool CanRemoveUrlCommand()
        {
            return ChromeBookmarks.URLs.Where(x => x.IsSelected == true).Count() > 0;
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
    }
}
