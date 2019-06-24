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
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Introduction", });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Overview of Interface",  });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Tree View",  } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Context Menus",  } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Selecting TreeView Items",  } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Loading JSON",  } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Errors",  });
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "About",  } );
            tempGuideData.TopicCollection.Add(new HelpTopic { Header = "Testing", HelpTextResource = "ChromeManagedBookmarksEditor.Images.HelpTopicImages.test.html" });

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
