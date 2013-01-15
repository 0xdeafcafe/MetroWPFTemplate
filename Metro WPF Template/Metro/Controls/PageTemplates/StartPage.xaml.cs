using System.Windows;
using System.Windows.Media.Animation;
using MetroWPFTemplate.Backend;

namespace MetroWPFTemplate.Metro.Controls.PageTemplates
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
	public partial class StartPage : IPage
    {
        public StartPage()
        {
            InitializeComponent();

            // Save the Start Page
            Settings.StartPage = this;

            // Setup the UI
            mainContent.Visibility = Visibility.Visible;
        }
        public bool Close() { return true; }
    }
}
