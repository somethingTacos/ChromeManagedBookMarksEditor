using System.Windows;
using System.Windows.Controls;

namespace ChromeManagedBookmarksEditor.CustomControls
{
    class SelectableTextBox : TextBox
    {
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SelectableTextBox), new PropertyMetadata(false));

        public bool IsSelected
        {
           get { return (bool)GetValue(IsSelectedProperty); }
           set { SetValue(IsSelectedProperty, value); }
        }
    }
}
