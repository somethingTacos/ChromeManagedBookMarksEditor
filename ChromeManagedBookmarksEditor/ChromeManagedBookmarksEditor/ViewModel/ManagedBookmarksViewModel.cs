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

        private async void OnSerializeCommand()
        {
            string ConvertedCode = string.Empty;
            changeInfo("Serializeing Tree...");
            ConvertedCode = await ConvertTreeToJSON(ChromeBookmarks);
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


        private async void OnLoadCommand()
        {
            try
            {
                ObservableCollection<ManagedBookmarks> tempBookMarks = new ObservableCollection<ManagedBookmarks>();
                _canLoad = false;
                LoadCommand.RaiseCanExecuteChanged();
                changeInfo("Loading JSON...");
                tempBookMarks = await LoadJSON(json.Code);

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

        private async Task<ObservableCollection<ManagedBookmarks>> LoadJSON(string json)
        {
            ObservableCollection<ManagedBookmarks> bookmarks = new ObservableCollection<ManagedBookmarks>();

            await Task.Run(() =>
            {
                bookmarks = _LoadJSON(json);
            });

            return bookmarks;
        }

        //this is going to be completly redone in the TreeConverter class
        private ObservableCollection<ManagedBookmarks> _LoadJSON(string json)
        {
            try
            {
                ObservableCollection<ManagedBookmarks> Bookmarks = new ObservableCollection<ManagedBookmarks>();
                ObservableCollection<Folder> FoldersList = new ObservableCollection<Folder>();

                var root = JsonConvert.DeserializeObject<ObservableCollection<RootFolder>>(json);
                var parsedfolders = JsonConvert.DeserializeObject<ObservableCollection<ParsedFolders>>(json);
                var urls = JsonConvert.DeserializeObject<ObservableCollection<URL>>(json);
                

                ObservableCollection<URL> clearNullURLs(ObservableCollection<URL> Urls)
                {
                    for (int i = 0; i < Urls.Count; i++)
                    {
                        if (Urls[i].Name == null || Urls[i].Url == null)
                        {
                            Urls.RemoveAt(i);
                            i--;
                        }
                    }

                    return Urls;
                }

                Folder iterateChildObjects(string subJson)
                {
                    try
                    {
                        var parsedSubfolders = JsonConvert.DeserializeObject<List<ParsedFolders>>(subJson);
                        Folder subfolder = new Folder();
                        Folder newfolder = new Folder();

                        for (int i = 0; i < parsedSubfolders.Count; i++)
                        {
                            if (parsedSubfolders[i].children != null)
                            {
                                if (parsedSubfolders[i].children.ToString().Contains("children"))
                                {
                                    newfolder = iterateChildObjects(parsedSubfolders[i].children.ToString());
                                }

                                if (newfolder.Name != null)
                                {
                                    subfolder.folders = new ObservableCollection<Folder>();
                                    subfolder.folders.Add(newfolder);
                                }

                                subfolder.Name = parsedSubfolders[i].Name.ToString();
                                subfolder.URLs = JsonConvert.DeserializeObject<ObservableCollection<URL>>(parsedSubfolders[i].children.ToString());

                                subfolder.URLs = clearNullURLs(subfolder.URLs);
                            }
                        }

                        return subfolder;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return null;
                    }

                }

                for (int i = 0; i < parsedfolders.Count; i++)
                {
                    if (parsedfolders[i].children != null)
                    {
                        Folder folder = new Folder();
                        folder.folders = new ObservableCollection<Folder>();

                        if (parsedfolders[i].children.ToString().Contains("children"))
                        {
                            Folder subfolder = new Folder();
                            subfolder = iterateChildObjects(parsedfolders[i].children.ToString());
                            folder.folders.Add(subfolder);
                        }

                        folder.Name = parsedfolders[i].Name.ToString();
                        folder.URLs = JsonConvert.DeserializeObject<ObservableCollection<URL>>(parsedfolders[i].children.ToString());
                        folder.URLs = clearNullURLs(folder.URLs);
                        FoldersList.Add(folder);
                    }
                }

                urls = clearNullURLs(urls);

                Bookmarks.Add(new ManagedBookmarks()
                {
                    toplevel_name = root[0].toplevel_name.ToString(),

                    Folders = FoldersList,

                    URLs = urls
                });

                return Bookmarks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private async Task<string> ConvertTreeToJSON(ObservableCollection<ManagedBookmarks> chromeBookmarks)
        {
            string chromeJSONcode = String.Empty;

            await Task.Run(() =>
            {
                chromeJSONcode = _ConvertTreeToJSON(chromeBookmarks);
            });

            return chromeJSONcode;
        }

        //this is going to be completly redone in the TreeConverter class
        private string _ConvertTreeToJSON(ObservableCollection<ManagedBookmarks> chromeBookmarks)
        {
            string ConvertedJSON = String.Empty;

            string toplevel_name = string.Format("[{{\"toplevel_name\":\"{0}\"}},", chromeBookmarks[0].toplevel_name.ToString());

            ConvertedJSON = toplevel_name;
            
            string iterateChildObject(int index, List<Folder> Folders, List<URL> Urls, bool isChildOfChild)
            {
                string childObjects = String.Empty;

                for (int o = 0; o < Folders.Count; o++)
                {
                    if (Folders[o].folders != null || Folders[o].URLs != null)
                    {
                        string Name = Folders[o].Name;

                        if (o is 0)
                        {
                            childObjects += string.Format("{{\"name\":\"{0}\",\"children\":[", Name);
                        }
                        else
                        {
                            childObjects += string.Format(",{{\"name\":\"{0}\",\"children\":[", Name);
                        }

                        List<Folder> childFolders = new List<Folder>();

                        if (Folders[o].folders != null)
                        {
                            foreach (Folder folder in Folders[o].folders)
                            {
                                childFolders.Add(folder);
                            }
                        }

                        List<URL> childURLs = new List<URL>();

                        foreach (URL url in Folders[o].URLs)
                        {
                            childURLs.Add(url);
                        }

                        string childItems = iterateChildObject(o, childFolders, childURLs, true);

                        childObjects += childItems;
                    }
                }
                

                for(int n = 0; n < Urls.Count; n++)
                {
                    string urlName = Urls[n].Name.ToString();
                    string url = Urls[n].Url.ToString();

                    if (index is 0 && n == 0 || n == 0)
                    {
                        childObjects += string.Format("{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
                    }
                    else
                    {
                        childObjects += string.Format(",{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
                    }
                }

                if(isChildOfChild ? index + 1 < Folders.Count || chromeBookmarks[0].Folders[index].URLs.Count != 0 : index + 1 < chromeBookmarks[0].Folders.Count)
                {
                    childObjects += "]},";
                }
                else
                {
                    childObjects += "]}";
                }

                return childObjects;
            }
           
            for (int i = 0; i < chromeBookmarks[0].Folders.Count; i++)
            {
                string Name = chromeBookmarks[0].Folders[i].Name.ToString();

                if(i is 0 || i + 1 == chromeBookmarks[0].Folders.Count)
                {
                    ConvertedJSON += string.Format("{{\"name\":\"{0}\",\"children\":[", Name);
                }
                else
                {
                    ConvertedJSON += string.Format(",{{\"name\":\"{0}\",\"children\":[", Name);
                }

                List<Folder> childFolders = new List<Folder>();


                foreach(Folder folder in chromeBookmarks[0].Folders[i].folders)
                {
                    childFolders.Add(folder);
                }

                List<URL> childURLs = new List<URL>();

                foreach(URL url in chromeBookmarks[0].Folders[i].URLs)
                {
                    childURLs.Add(url);
                }

                string subFolders = iterateChildObject(i, childFolders, childURLs, false);

                ConvertedJSON += subFolders;
            }
            

            for (int i = 0; i < chromeBookmarks[0].URLs.Count; i++)
            {
                string urlName = chromeBookmarks[0].URLs[i].Name.ToString();
                string url = chromeBookmarks[0].URLs[i].Url.ToString();

                if (i is 0 ? chromeBookmarks[0].Folders.Count is 0 : false)
                {
                    ConvertedJSON += string.Format("{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
                }
                else
                {
                    ConvertedJSON += string.Format(",{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
                }
            }

            ConvertedJSON += "]";

            return ConvertedJSON;
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

        // ADD FOLDER
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

        // ADD URL
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

        // CLEAR ALL
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
