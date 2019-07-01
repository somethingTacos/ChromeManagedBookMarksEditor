using System.Collections.ObjectModel;
using PropertyChanged;

namespace ChromeManagedBookmarksEditor.Model
{
    public class HelpModel { }

    [ImplementPropertyChanged]
    public class HelpTopic
    {
        public string Header { get; set; } = "";
        public string HelpTextResource { get; set; } = "";
    }

    [ImplementPropertyChanged]
    public class Guide
    {
        public ObservableCollection<HelpTopic> TopicCollection { get; set; } = new ObservableCollection<HelpTopic>();
        public string CurrentHelpInfo { get; set; } = "";
    }
}
