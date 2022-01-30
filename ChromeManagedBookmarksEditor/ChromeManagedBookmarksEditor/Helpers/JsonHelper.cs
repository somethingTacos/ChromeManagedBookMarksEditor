using ChromeManagedBookmarksEditor.Models.Results;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ChromeManagedBookmarksEditor.Models;
using System.Linq;
using ChromeManagedBookmarksEditor.Interfaces;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public static class JsonHelper
    {
        /// <summary>
        /// Saves an object to a json file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="FilePath"></param>
        /// <returns>A <see cref="GenericResult"/> that contines the unformatted json string in <see cref="GenericResult.Data"/></returns>
        public static GenericResult SaveToFile<T>(T Data, string FilePath) where T : class
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(Data, Formatting.Indented);

                File.WriteAllText(FilePath, jsonData);

                string nonFormatterdJson = JValue.Parse(jsonData).ToString(Formatting.None);

                return GenericResult.FromSuccess("", nonFormatterdJson);
            }
            catch(Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }

        public static string Serialize<T>(T Data)
        {
            try
            {
                return JsonConvert.SerializeObject(Data);
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public static GenericResult LoadFromFile<T>(string FilePath) where T : class
        {
            try
            {
                string jsonData = File.ReadAllText(FilePath);

                return Deserialize<T>(jsonData);
            }
            catch(Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }

        public static GenericResult Deserialize<T>(string json) where T : class
        {
            try
            {
                T data = JsonConvert.DeserializeObject<T>(json);

                return GenericResult.FromSuccess("", data);
            }
            catch(Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }

        private static object SwitchDataKind(JToken token) =>
            (token.Values()?.Count() == 1, token.Values()?.Count() > 1 && token.Values()?.Last()?.Type == JTokenType.Array) switch
            {
                (true, false) => token.ToObject<ManagedBookmarks>(),
                (_, true) => token.ToObject<Folder>(),
                (_, false) => token.ToObject<Bookmark>()
            };

        public static GenericResult ConvertDataToTypes(object[] Data)
        {
            List<object> convertedData = new List<object>();

            try
            {
                foreach (object dataItem in Data)
                {
                    if (dataItem is JToken token)
                    {
                        var item = SwitchDataKind(token);

                        if(item is ManagedBookmarks mbm && mbm.RootName != "")
                        {
                            convertedData.Add(item);
                            continue;
                        }

                        if(item is Folder f && f.Name != "")
                        {
                            if (f.Children.Count > 0)
                            {
                                var result = ConvertDataToTypes(f.Children.ToArray());

                                if(result.Succeeded && result.HasData && result.Data is object[] childObjects)
                                {
                                    f.Children.Clear();

                                    foreach(object obj in childObjects)
                                    {
                                        if(obj is IChild child)
                                        {
                                            child.Parent = f;
                                            f.Children.Add(child);
                                        }
                                    }
                                }
                            }

                            convertedData.Add(item);
                            continue;
                        }
                        if(item is Bookmark b && b.Name != "")
                        {
                            convertedData.Add(item);
                            continue;
                        }
                    }
                }

                if (convertedData.Count > 0) return GenericResult.FromSuccess("", convertedData.ToArray());

                return GenericResult.FromError("Could not convert data");
            }
            catch(Exception ex)
            {
                return GenericResult.FromException(ex);
            }
        }
    }
}
