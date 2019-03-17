using System.Collections.ObjectModel;

using PropertyChanged;

namespace LinkHierarchy2JSON.Model
{
    public class HelpModel { }

    [ImplementPropertyChanged]
    public class HelpTopic
    {
        public string Header { get; set; } = "";
        public int HelpImageNumber { get; set; } = 0;
    }

    [ImplementPropertyChanged]
    public class Guide
    {
        public ObservableCollection<HelpTopic> TopicCollection { get; set; } = new ObservableCollection<HelpTopic>();
        public string CurrentHelpImage { get; set; } = "";
    }
}

// OLD DATA MODEL

//    public class HelpTopic : INotifyPropertyChanged
//    {
//        private string _header;

//        public string Header
//        {
//            get
//            {
//                return _header;
//            }
//            set
//            {
//                _header = value;
//                RaisePropertyChanged("Header");
//            }
//        }
//        private int _helpImageNumber;

//        public int HelpImageNumber
//        {
//            get
//            {
//                return _helpImageNumber;
//            }
//            set
//            {
//                _helpImageNumber = value;
//                RaisePropertyChanged("HelpImage");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }

//    public class Guide : INotifyPropertyChanged
//    {
//        private ObservableCollection<HelpTopic> _topicCollection;
//        private string _currentHelpImage;

//        public ObservableCollection<HelpTopic> TopicCollection
//        {
//            get
//            {
//                return _topicCollection;
//            }
//            set
//            {
//                _topicCollection = value;
//                RaisePropertyChanged("TopicCollection");
//            }
//        }

//        public string CurrentHelpImage
//        {
//            get
//            {
//                return _currentHelpImage;
//            }
//            set
//            {
//                _currentHelpImage = value;
//                RaisePropertyChanged("CurrentHelpImage");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void RaisePropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//    }
//}
