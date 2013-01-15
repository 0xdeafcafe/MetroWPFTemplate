using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroWPFTemplate.Backend;

namespace MetroWPFTemplate.Metro.Dialogs
{
    public class MetroException
    {
        /// <summary>
        /// Show the exception error dialog window.
        /// </summary>
        /// <param name="ex">The exception to pass into the dialog.</param>
        public static void Show(Exception ex)
        {
            // Run it though the dictionary, see if it can be made more user-friendlyKK
            
            Settings.HomeWindow.ShowMask();
            ControlDialogs.Exception exceptionDialog = new ControlDialogs.Exception(ex);
            exceptionDialog.Owner = Settings.HomeWindow;
            exceptionDialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            exceptionDialog.ShowDialog();
            Settings.HomeWindow.HideMask();
        }
    }
}