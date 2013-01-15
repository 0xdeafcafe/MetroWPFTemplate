using MetroWPFTemplate.Metro.Controls.PageTemplates;
using MetroWPFTemplate.Metro.Dialogs;
using MetroWPFTemplate.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace MetroWPFTemplate.Backend
{
    public class Settings
    {
        public static void LoadSettings(bool applyThemeAswell = false)
        {
            // Declare Registry
			var keyApp = Registry.CurrentUser.CreateSubKey("Software\\Xeraxic\\Metro WPF Template\\ApplicationSettings\\");

	        if (keyApp != null) ApplicationAccent = (Accents)keyApp.GetValue("accent", 0);
	        if (applyThemeAswell)
                ApplyAccent();

	        if (keyApp == null) return;
	        ApplicationSizeWidth = Convert.ToSingle(keyApp.GetValue("SizeWidth", 1100));
	        ApplicationSizeHeight = Convert.ToSingle(keyApp.GetValue("SizeHeight", 600));
	        ApplicationSizeMaximize = Convert.ToBoolean(keyApp.GetValue("SizeMaxamize", false));
        }
        public static void UpdateSettings(bool applyThemeAswell = false)
        {
            // Declare Registry
			var keyApp = Registry.CurrentUser.CreateSubKey("Software\\Xeraxic\\Metro WPF Template\\ApplicationSettings\\");

	        if (keyApp != null) keyApp.SetValue("accent", (int)ApplicationAccent);
	        if (applyThemeAswell)
                ApplyAccent();

	        if (keyApp == null) return;
	        keyApp.SetValue("SizeWidth", ApplicationSizeWidth);
	        keyApp.SetValue("SizeHeight", ApplicationSizeHeight);
	        keyApp.SetValue("SizeMaxamize", ApplicationSizeMaximize);
        }


        public static void ApplyAccent()
        {
			var theme = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Enum.Parse(typeof(Accents), ApplicationAccent.ToString()).ToString());
            try
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/MetroWPFTemplate;component/Metro/Themes/" + theme + ".xaml", UriKind.Relative) });
            }
            catch
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/MetroWPFTemplate;component/Metro/Themes/Blue.xaml", UriKind.Relative) });
            }
        }
        public static Accents ApplicationAccent = Accents.Blue;

        public static double ApplicationSizeWidth = 1100;
        public static double ApplicationSizeHeight = 600;
        public static bool ApplicationSizeMaximize = false;

        public static Home HomeWindow = null;
        public static StartPage StartPage = null;
        public static IList<string> OpenedSaves = new List<string>();

        public enum Accents
        {
            Blue,
            Purple,
            Orange,
            Green
        }
    }

    public class TempStorage
    {
        public static MetroMessageBox.MessageBoxResults MessageBoxButtonStorage;
    }
}