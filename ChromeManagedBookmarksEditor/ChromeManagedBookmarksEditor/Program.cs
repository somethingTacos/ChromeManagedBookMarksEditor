using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;
using System;
using System.Reflection;

namespace ChromeManagedBookmarksEditor
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

            return AppBuilder.Configure<App>()
                   .UsePlatformDetect()
                   .UseReactiveUI()
                   .LogToTrace()
                   .UseReactiveUI();
        }
    }
}
