using ChromeManagedBookmarksEditor.Model;
using System.Collections.ObjectModel;
using ChromeManagedBookmarksEditor.Helpers;
using System.Windows;

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

            //NOTE - make sure all HelpTextResource's are embedded resources
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Introduction", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.Introduction.html" });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Interface Overview", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.InterfaceOverview.html" });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Object Interactions", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.ObjectInteractions.html" } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Loading JSON", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.LoadingJSON.html" } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "About", HelpTextResource = "ChromeManagedBookmarksEditor.HelpDocuments.About.html" } );

            tempGuideData.CurrentHelpInfo = tempGuideData.TopicCollection[0].HelpTextResource;

            HelpGuide = tempGuideData;
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
