using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public static class ChromeJSONConverter
    {

    }
}


//All of this non-sense is going to be reworked, just moving it to the correct class for now.

//private async Task<ObservableCollection<ManagedBookmarks>> LoadJSON(string json)
//{
//    ObservableCollection<ManagedBookmarks> bookmarks = new ObservableCollection<ManagedBookmarks>();

//    await Task.Run(() =>
//    {
//        bookmarks = _LoadJSON(json);
//    });

//    return bookmarks;
//}

////this is going to be completly redone in the TreeConverter class
//private ObservableCollection<ManagedBookmarks> _LoadJSON(string json)
//{
//    try
//    {
//        ObservableCollection<ManagedBookmarks> Bookmarks = new ObservableCollection<ManagedBookmarks>();
//        ObservableCollection<Folder> FoldersList = new ObservableCollection<Folder>();

//        var root = JsonConvert.DeserializeObject<ObservableCollection<RootFolder>>(json);
//        var parsedfolders = JsonConvert.DeserializeObject<ObservableCollection<ParsedFolders>>(json);
//        var urls = JsonConvert.DeserializeObject<ObservableCollection<URL>>(json);


//        ObservableCollection<URL> clearNullURLs(ObservableCollection<URL> Urls)
//        {
//            for (int i = 0; i < Urls.Count; i++)
//            {
//                if (Urls[i].Name == null || Urls[i].Url == null)
//                {
//                    Urls.RemoveAt(i);
//                    i--;
//                }
//            }

//            return Urls;
//        }

//        Folder iterateChildObjects(string subJson)
//        {
//            try
//            {
//                var parsedSubfolders = JsonConvert.DeserializeObject<List<ParsedFolders>>(subJson);
//                Folder subfolder = new Folder();
//                Folder newfolder = new Folder();

//                for (int i = 0; i < parsedSubfolders.Count; i++)
//                {
//                    if (parsedSubfolders[i].children != null)
//                    {
//                        if (parsedSubfolders[i].children.ToString().Contains("children"))
//                        {
//                            newfolder = iterateChildObjects(parsedSubfolders[i].children.ToString());
//                        }

//                        if (newfolder.Name != null)
//                        {
//                            subfolder.folders = new ObservableCollection<Folder>();
//                            subfolder.folders.Add(newfolder);
//                        }

//                        subfolder.Name = parsedSubfolders[i].Name.ToString();
//                        subfolder.URLs = JsonConvert.DeserializeObject<ObservableCollection<URL>>(parsedSubfolders[i].children.ToString());

//                        subfolder.URLs = clearNullURLs(subfolder.URLs);
//                    }
//                }

//                return subfolder;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//                return null;
//            }

//        }

//        for (int i = 0; i < parsedfolders.Count; i++)
//        {
//            if (parsedfolders[i].children != null)
//            {
//                Folder folder = new Folder();
//                folder.folders = new ObservableCollection<Folder>();

//                if (parsedfolders[i].children.ToString().Contains("children"))
//                {
//                    Folder subfolder = new Folder();
//                    subfolder = iterateChildObjects(parsedfolders[i].children.ToString());
//                    folder.folders.Add(subfolder);
//                }

//                folder.Name = parsedfolders[i].Name.ToString();
//                folder.URLs = JsonConvert.DeserializeObject<ObservableCollection<URL>>(parsedfolders[i].children.ToString());
//                folder.URLs = clearNullURLs(folder.URLs);
//                FoldersList.Add(folder);
//            }
//        }

//        urls = clearNullURLs(urls);

//        Bookmarks.Add(new ManagedBookmarks()
//        {
//            toplevel_name = root[0].toplevel_name.ToString(),

//            Folders = FoldersList,

//            URLs = urls
//        });

//        return Bookmarks;
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message);
//        return null;
//    }
//}

//private async Task<string> ConvertTreeToJSON(ObservableCollection<ManagedBookmarks> chromeBookmarks)
//{
//    string chromeJSONcode = String.Empty;

//    await Task.Run(() =>
//    {
//        chromeJSONcode = _ConvertTreeToJSON(chromeBookmarks);
//    });

//    return chromeJSONcode;
//}

////this is going to be completly redone in the TreeConverter class
//private string _ConvertTreeToJSON(ObservableCollection<ManagedBookmarks> chromeBookmarks)
//{
//    string ConvertedJSON = String.Empty;

//    string toplevel_name = string.Format("[{{\"toplevel_name\":\"{0}\"}},", chromeBookmarks[0].toplevel_name.ToString());

//    ConvertedJSON = toplevel_name;

//    string iterateChildObject(int index, List<Folder> Folders, List<URL> Urls, bool isChildOfChild)
//    {
//        string childObjects = String.Empty;

//        for (int o = 0; o < Folders.Count; o++)
//        {
//            if (Folders[o].folders != null || Folders[o].URLs != null)
//            {
//                string Name = Folders[o].Name;

//                if (o is 0)
//                {
//                    childObjects += string.Format("{{\"name\":\"{0}\",\"children\":[", Name);
//                }
//                else
//                {
//                    childObjects += string.Format(",{{\"name\":\"{0}\",\"children\":[", Name);
//                }

//                List<Folder> childFolders = new List<Folder>();

//                if (Folders[o].folders != null)
//                {
//                    foreach (Folder folder in Folders[o].folders)
//                    {
//                        childFolders.Add(folder);
//                    }
//                }

//                List<URL> childURLs = new List<URL>();

//                foreach (URL url in Folders[o].URLs)
//                {
//                    childURLs.Add(url);
//                }

//                string childItems = iterateChildObject(o, childFolders, childURLs, true);

//                childObjects += childItems;
//            }
//        }


//        for (int n = 0; n < Urls.Count; n++)
//        {
//            string urlName = Urls[n].Name.ToString();
//            string url = Urls[n].Url.ToString();

//            if (index is 0 && n == 0 || n == 0)
//            {
//                childObjects += string.Format("{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
//            }
//            else
//            {
//                childObjects += string.Format(",{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
//            }
//        }

//        if (isChildOfChild ? index + 1 < Folders.Count || chromeBookmarks[0].Folders[index].URLs.Count != 0 : index + 1 < chromeBookmarks[0].Folders.Count)
//        {
//            childObjects += "]},";
//        }
//        else
//        {
//            childObjects += "]}";
//        }

//        return childObjects;
//    }

//    for (int i = 0; i < chromeBookmarks[0].Folders.Count; i++)
//    {
//        string Name = chromeBookmarks[0].Folders[i].Name.ToString();

//        if (i is 0 || i + 1 == chromeBookmarks[0].Folders.Count)
//        {
//            ConvertedJSON += string.Format("{{\"name\":\"{0}\",\"children\":[", Name);
//        }
//        else
//        {
//            ConvertedJSON += string.Format(",{{\"name\":\"{0}\",\"children\":[", Name);
//        }

//        List<Folder> childFolders = new List<Folder>();


//        foreach (Folder folder in chromeBookmarks[0].Folders[i].folders)
//        {
//            childFolders.Add(folder);
//        }

//        List<URL> childURLs = new List<URL>();

//        foreach (URL url in chromeBookmarks[0].Folders[i].URLs)
//        {
//            childURLs.Add(url);
//        }

//        string subFolders = iterateChildObject(i, childFolders, childURLs, false);

//        ConvertedJSON += subFolders;
//    }


//    for (int i = 0; i < chromeBookmarks[0].URLs.Count; i++)
//    {
//        string urlName = chromeBookmarks[0].URLs[i].Name.ToString();
//        string url = chromeBookmarks[0].URLs[i].Url.ToString();

//        if (i is 0 ? chromeBookmarks[0].Folders.Count is 0 : false)
//        {
//            ConvertedJSON += string.Format("{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
//        }
//        else
//        {
//            ConvertedJSON += string.Format(",{{\"name\":\"{0}\",\"url\":\"{1}\"}}", urlName, url);
//        }
//    }

//    ConvertedJSON += "]";

//    return ConvertedJSON;
//}