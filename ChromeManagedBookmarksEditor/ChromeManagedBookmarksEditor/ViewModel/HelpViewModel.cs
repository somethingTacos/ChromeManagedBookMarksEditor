using ChromeManagedBookmarksEditor.Model;
using System.Collections.ObjectModel;
using ChromeManagedBookmarksEditor.Helpers;
using System.Windows;
using System.Linq;
using System.Reflection;
using System.IO;
using System;
using Microsoft.Win32;

namespace ChromeManagedBookmarksEditor.ViewModel
{
    //This is all going to change... I think... I would like something a little more flexable and I hate using images, but we shall see.
    public class HelpViewModel
    {
        public Guide HelpGuide { get; set; }
        private NavigationViewModel _navigationViewModel { get; set; }
        public MyICommand BackCommand { get; set; }

        public HelpViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            BackCommand = new MyICommand(onBackCommand);
            Guide tempGuideData = new Guide();
            tempGuideData.TopicCollection = new ObservableCollection<HelpTopic>();

            string blah = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            CheckHelpDocImagesExist();

            //NOTE - make sure all HelpTextResource's are embedded resources
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Introduction", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.Introduction.html" });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Interface Overview", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.InterfaceOverview.html" });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Object Interactions", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.ObjectInteractions.html" } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "About", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.About.html" } );

            tempGuideData.CurrentHelpInfo = tempGuideData.TopicCollection[0].HelpTextResource;

            HelpGuide = tempGuideData;
        }

        private void CheckHelpDocImagesExist() // <-- this will need to be fully tested after the installers are created.  <-- -- NOTE
        {
            void WriteResourceToFile(string resourceName, string filePath)
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        resource.CopyTo(file);
                    }
                }
            }

            string CopyDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ChromeManagedBookmarksEditor\\HelpDocImages";

            string[] HelpDocImageList = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            HelpDocImageList = HelpDocImageList.Where(x => x.Contains("HelpDocImages")).ToArray();

            string[] ImageNamesList = new string[HelpDocImageList.Count()];

            for (int i = 0; i < HelpDocImageList.Count(); i++)
            {
                ImageNamesList[i] = HelpDocImageList[i].Remove(0, 50);
            }

            if(!Directory.Exists(CopyDir))
            {
                Directory.CreateDirectory(CopyDir);
            }

            for (int i = 0; i < HelpDocImageList.Count(); i++)
            {
                string FileFullName = CopyDir + $"\\{ImageNamesList[i]}";
                if(!File.Exists(FileFullName))
                {
                    WriteResourceToFile(HelpDocImageList[i], FileFullName);
                }
            }
        }

        public void onBackCommand(object parameter)
        {
            _navigationViewModel.SelectedViewModel = new ManagedBookmarksViewModel(_navigationViewModel);
        }

        private HelpTopic _currentTopic;

        public HelpTopic CurrentTopic
        {
            get
            {
                return _currentTopic;
            }
            set
            {
                _currentTopic = value;
                HelpGuide.CurrentHelpInfo =  _currentTopic.HelpTextResource;
            }
        }
    }
}
