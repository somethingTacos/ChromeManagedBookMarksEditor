using ReactiveUI;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; set; } = new RoutingState();

        public MainWindowViewModel()
        {
            Router.Navigate.Execute(new StartupMenuViewModel(this));
        }
    }
}
