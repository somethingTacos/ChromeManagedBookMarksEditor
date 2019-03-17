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
