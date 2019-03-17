using System.Windows;
using LinkHierarchy2JSON.ViewModel;

namespace LinkHierarchy2JSON
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewmodel = new NavigationViewModel();
            viewmodel.SelectedViewModel = new ManagedBookmarksViewModel(viewmodel);
            this.DataContext = viewmodel;
        }
    }
}


