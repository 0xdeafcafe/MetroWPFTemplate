using System.Windows;
using MetroWPFTemplate.Backend;

namespace MetroWPFTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load Settings
            Settings.LoadSettings(true);

            // Create Closing Method
            Current.Exit += (o, args) =>
            {
                // Update Settings with Window Width/Height
                Settings.ApplicationSizeMaximize = (Settings.HomeWindow.WindowState == WindowState.Maximized);
                if (!Settings.ApplicationSizeMaximize)
                {
                    Settings.ApplicationSizeWidth = Settings.HomeWindow.Width;
                    Settings.ApplicationSizeHeight = Settings.HomeWindow.Height;
                }

                // Save Settings
                Settings.UpdateSettings();
            };

            // Global Exception Catching
#if !DEBUG
            Application.Current.DispatcherUnhandledException += (o, args) =>
                {
                    MetroException.Show((Exception)args.Exception);

                    args.Handled = true;
                };
#endif
        }
    }
}