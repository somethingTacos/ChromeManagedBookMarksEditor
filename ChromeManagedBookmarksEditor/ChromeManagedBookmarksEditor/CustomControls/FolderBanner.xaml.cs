using System.Windows;
using System.Windows.Controls;

namespace ChromeManagedBookmarksEditor.CustomControls
{
    /// <summary>
    /// Interaction logic for NewFolderBanner.xaml
    /// </summary>
    public partial class FolderBanner : UserControl
    {
        public FolderBanner()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsVisibleBannerProperty =
            DependencyProperty.Register("IsVisibleBanner", typeof(bool), typeof(FolderBanner), new PropertyMetadata(false));

        public bool IsVisibleBanner
        {
            get { return (bool)GetValue(IsVisibleBannerProperty); }
            set { SetValue(IsVisibleBannerProperty, value); }
        }
    }
}
