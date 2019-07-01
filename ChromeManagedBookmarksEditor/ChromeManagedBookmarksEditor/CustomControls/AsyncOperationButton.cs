using System.Windows;
using System.Windows.Controls;

namespace ChromeManagedBookmarksEditor.CustomControls
{
    class AsyncOperationButton : Button
    {
        public static readonly DependencyProperty IsPerformingOperationProperty =
            DependencyProperty.Register("IsPerformingOperation", typeof(bool), typeof(SelectableTextBox), new PropertyMetadata(false));

        public bool IsPerformingOperation
        {
            get { return (bool)GetValue(IsPerformingOperationProperty); }
            set { SetValue(IsPerformingOperationProperty, value); }
        }
    }
}
