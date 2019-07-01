using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ChromeManagedBookmarksEditor.CustomControls
{
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;

                var assembly = Assembly.GetExecutingAssembly();
                //var names = assembly.GetManifestResourceNames(); //NOTE - use this to check embedded resource names.

                if (uri != null && uri != "")
                {
                    using (Stream stream = assembly.GetManifestResourceStream(uri))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string htmlString = reader.ReadToEnd();
                        browser.NavigateToString(htmlString);
                    }
                }
            }
        }

    }
}
