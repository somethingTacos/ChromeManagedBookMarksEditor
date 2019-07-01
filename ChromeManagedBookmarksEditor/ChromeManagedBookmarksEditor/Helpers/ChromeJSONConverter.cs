using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChromeManagedBookmarksEditor.Model;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public static class ChromeJSONConverter
    {
        // Async methods to convert ManagedBookmarks object into JSON code
        public static async Task<string> ConvertToJSON(Folder RootFolder)
        {
            string ReturnableJSON = string.Empty;

            await Task.Run(() => 
            {
                ReturnableJSON = _ConvertToJSON(RootFolder);
            });

            return ReturnableJSON;
        }

        private static string _ConvertToJSON(Folder RootFolder)
        {
            string convertedJSON = string.Empty;

            string topLevelName = $"[{{\"toplevel_name\":\"{RootFolder.Name}\"}},";

            convertedJSON = topLevelName;

            ObservableCollection<string> JSONCollection = new ObservableCollection<string>();

            string GetFolderJSONContent(Folder folder)
            {
                ObservableCollection<string> folderContents = new ObservableCollection<string>();
                if (folder.folders.Count > 0)
                {
                    foreach (Folder subfolder in folder.folders)
                    {
                        folderContents.Add($"{{\"name\":\"{subfolder.Name}\",\"children\":[]}}");
                    }
                }

                if (folder.URLs.Count > 0)
                {
                    foreach (URL url in folder.URLs)
                    {
                        folderContents.Add($"{{\"name\":\"{url.Name}\",\"url\":\"{url.Url}\"}}");
                    }
                }

                string joinedContents = String.Join(",", folderContents);
                if(joinedContents == "" && folder.FolderIndex != 0) { joinedContents = "EMPTY"; }
                return joinedContents;
            }

            void IterateSubFolders(Folder folder)
            {
                JSONCollection.Add(GetFolderJSONContent(folder));
                if (folder.folders.Count > 0)
                {
                    foreach (Folder subFolder in folder.folders)
                    {
                        IterateSubFolders(subFolder);
                    }
                }
            }

            //Get RootFolder content
            JSONCollection.Add(GetFolderJSONContent(RootFolder));

            //iterate RootFolders.folders
            if (RootFolder.folders.Count > 0)
            {
                foreach (Folder subfolder in RootFolder.folders)
                {
                    IterateSubFolders(subfolder);
                }
            }

            convertedJSON += JSONCollection[0];

            //replace folder children [] with actual contents
            for(int i = 1; i < JSONCollection.Count(); i++)
            {
                var regex = new Regex(Regex.Escape("[]"));
                convertedJSON = regex.Replace(convertedJSON, $"[{JSONCollection[i]}]", 1);
            }

            convertedJSON = convertedJSON.Replace("[EMPTY]", "[]");
            convertedJSON += "]";

            return convertedJSON;
        }

        // Async Methods to parse JSON code into a ManagedBookmarks object
        public static async Task<ManagedBookmarks> ParseJSON(string JSONCode)
        {
            ManagedBookmarks ReturnableManagedBookmarks = new ManagedBookmarks();

            await Task.Run(() =>
            {
                ReturnableManagedBookmarks = _ParseJSON(JSONCode);
            });

            return ReturnableManagedBookmarks;
        }

        private static ManagedBookmarks _ParseJSON(string JSONCode)
        {
            ManagedBookmarks ParsedBookmarks = new ManagedBookmarks();
            try
            {
                string StrippedJSONData = JSONCode.Replace("},{", "}|{");
                StrippedJSONData = StrippedJSONData.Replace("\",\"", "\"|\"");
                StrippedJSONData = StrippedJSONData.Replace("{", "");
                StrippedJSONData = StrippedJSONData.Replace("}", "");
                StrippedJSONData = StrippedJSONData.Replace("\"", "");
                StrippedJSONData = StrippedJSONData.Remove(0, 1);
                StrippedJSONData = StrippedJSONData.Remove(StrippedJSONData.Count() - 1, 1);

                List<string> JSONDataList = new List<string>();
                JSONDataList = StrippedJSONData.Split('|').ToList<string>();

                Folder WorkingFolder = new Folder();
                string lastName = "";

                foreach (string data in JSONDataList)
                {
                    string[] CurrentData = data.Split(':');

                    switch (CurrentData[0])
                    {
                        case "toplevel_name":
                            {
                                ParsedBookmarks.RootFolder.Name = CurrentData[1];
                                ParsedBookmarks.RootFolder.FolderIndex = 0;
                                WorkingFolder = ParsedBookmarks.RootFolder;
                                break;
                            }
                        case "children":
                            {
                                Folder newFolder = new Folder { Name = lastName };
                                newFolder.Parent = WorkingFolder;
                                newFolder.FolderIndex = WorkingFolder.FolderIndex + 1;
                                WorkingFolder.folders.Add(newFolder);

                                if (data != "children:[]")
                                {
                                    lastName = data.Substring(10).Split(':')[1];
                                    WorkingFolder = WorkingFolder.folders.Where(x => x == newFolder).FirstOrDefault();
                                }
                                break;
                            }
                        case "name":
                            {
                                lastName = CurrentData[1];
                                break;
                            }
                        case "url":
                            {
                                URL newURL = new URL { Name = lastName, Url = String.Join(":", CurrentData) };

                                bool FolderEnd = CurrentData[CurrentData.Count() - 1].EndsWith("]");
                                newURL.Url = newURL.Url.Remove(0, 4);

                                WorkingFolder.URLs.Add(newURL);

                                if (FolderEnd) { WorkingFolder = WorkingFolder.Parent; }

                                break;
                            }
                    }
                }
            }
            catch(Exception ex) { }

            return ParsedBookmarks;
        }
    }
}