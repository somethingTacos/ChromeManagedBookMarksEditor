using System.Windows.Input;

namespace ChromeManagedBookmarksEditor.Helpers
{
    public interface IRaiseCanExecuteChanged
    {
        void RaiseCanExecuteChanged();
    }

    // And an extension method to make it easy to raise changed events
    public static class CommandExtensions
    {
        public static void RaiseCanExecuteChanged(this ICommand command)
        {
            var canExecuteChanged = command as IRaiseCanExecuteChanged;

            if (canExecuteChanged != null)
                canExecuteChanged.RaiseCanExecuteChanged();
        }
    }
}