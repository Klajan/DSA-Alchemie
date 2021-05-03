using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Alchemie.UI.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        public UpdateWindow(Release release)
        {
            _release = release;
            DataContext = Release;
            InitializeComponent();
        }

        private readonly Release _release = new();
        public Release Release { get => _release; }
        public static string CurrentVersion { get => UpdateChecker.CurrentVersion; }

        private static void Navigate(Uri uri)
        {
            if (uri.IsAbsoluteUri && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Navigate(e.Uri);
            e.Handled = true;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.Enter)
            {
                Navigate(Release.Url);
                this.Close();
            }
        }
    }
}