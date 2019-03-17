using LinkHierarchy2JSON.Model;
using System.Collections.ObjectModel;

namespace LinkHierarchy2JSON.ViewModel
{
    //This is all going to change... I think... I would like something a little more flexable and I hate using images, but we shall see.
    public class HelpViewModel
    {
        public Guide HelpGuide { get; set; }
        private NavigationViewModel _navigationViewModel { get; set; }
        public HelpViewModel(NavigationViewModel navigationViewModel)
        {
            _navigationViewModel = navigationViewModel;
            Guide blah = new Guide();
            blah.TopicCollection = new ObservableCollection<HelpTopic>();

            blah.TopicCollection.Add(new HelpTopic { Header = "Introduction", HelpImageNumber = 0 });
            blah.TopicCollection.Add(new HelpTopic { Header = "Overview of Interface", HelpImageNumber = 1 });
            blah.TopicCollection.Add(new HelpTopic { Header = "Tree View", HelpImageNumber = 2 } );
            blah.TopicCollection.Add(new HelpTopic { Header = "Context Menus", HelpImageNumber = 3 } );
            blah.TopicCollection.Add(new HelpTopic { Header = "Selecting TreeView Items", HelpImageNumber = 4 } );
            blah.TopicCollection.Add(new HelpTopic { Header = "Loading JSON", HelpImageNumber = 5 } );
            blah.TopicCollection.Add(new HelpTopic { Header = "Errors", HelpImageNumber = 6 });
            blah.TopicCollection.Add(new HelpTopic { Header = "About", HelpImageNumber = 7 } );

            blah.CurrentHelpImage = SetCurrentHelpImage(0);

            HelpGuide = blah;
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
                HelpGuide.CurrentHelpImage = SetCurrentHelpImage(_currentTopic.HelpImageNumber);
            }
        }

        private string SetCurrentHelpImage(int imageNumber)
        {
            return "/ChromeManagedBookmarksEditor;component/Images/HelpTopicImages/" + imageNumber.ToString() + ".jpg";
        }
    }
}
