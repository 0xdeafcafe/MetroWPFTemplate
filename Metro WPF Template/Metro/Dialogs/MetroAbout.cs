using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroWPFTemplate.Backend;

namespace MetroWPFTemplate.Metro.Dialogs
{
    public class MetroAbout
    {
        /// <summary>
        /// Show the About Window
        /// </summary>
        public static void Show()
        {
            Settings.HomeWindow.ShowMask();
            ControlDialogs.About about = new ControlDialogs.About();
            about.Owner = Settings.HomeWindow;
            about.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            about.ShowDialog();
            Settings.HomeWindow.HideMask();
        }
    }
}
