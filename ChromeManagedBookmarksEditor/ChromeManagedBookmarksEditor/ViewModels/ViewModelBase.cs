using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ChromeManagedBookmarksEditor.ViewModels
{
    public class ViewModelBase : ReactiveObject, IActivatableViewModel, IRoutableViewModel
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public string? UrlPathSegment => Guid.NewGuid().ToString().Substring(0, 7);

        public ReactiveCommand<Unit, Unit> OpenSettingsCommand { get; set; }

        public WindowNotificationManager NotificationManager => Locator.Current.GetService<WindowNotificationManager>();

        public IScreen HostScreen { get; }

        /// <summary>
        /// Delay the return of the viewmodel
        /// </summary>
        /// <param name="Milliseconds">The amount of time in milliseconds to delay</param>
        /// <returns>The viewmodel after the delay time</returns>
        /// <remarks>Useful to delay the navigation to another view. For instance, to allow an animation to complete.</remarks>
        private async Task<ViewModelBase> WithDelay(int Milliseconds)
        {
            await Task.Delay(Milliseconds);

            return this;
        }

        /// <summary>
        /// Navigate to another viewmodel after a delay
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <param name="Milliseconds"></param>
        /// <returns></returns>
        public async Task NavigateToWithDelay(ViewModelBase ViewModel, int Milliseconds)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                HostScreen.Router.Navigate.Execute(await ViewModel.WithDelay(Milliseconds));
            });
        }

        /// <summary>
        /// Navigate to another viewmodel
        /// </summary>
        /// <param name="ViewModel"></param>
        public void NavigateTo(ViewModelBase ViewModel)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                HostScreen.Router.Navigate.Execute(ViewModel);
            });
        }

        /// <summary>
        /// Navigate to the previous viewmodel
        /// </summary>
        public void NavigateBack()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                HostScreen.Router.NavigateBack.Execute();
            });
        }

        public ViewModelBase(IScreen Host)
        {
            HostScreen = Host;

            OpenSettingsCommand = ReactiveCommand.Create(() =>
            {
                NavigateTo(new SettingsViewModel(HostScreen));
            });
        }

        /// <summary>
        /// A helper method to send a generic notification
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        /// <param name="Experation"></param>
        /// <param name="OnClick"></param>
        /// <param name="OnClose"></param>
        public void SendNotification(string Title, string Message, NotificationType Type = NotificationType.Information, TimeSpan? Experation = null, Action? OnClick = null, Action? OnClose = null)
        {
            NotificationManager.Show(new Avalonia.Controls.Notifications.Notification(Title, Message, Type, Experation, OnClick, OnClose));
        }
    }
}
