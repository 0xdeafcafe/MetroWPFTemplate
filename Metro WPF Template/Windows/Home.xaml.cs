using CloseableTabItemDemo;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using MetroWPFTemplate.Backend;
using MetroWPFTemplate.Metro.Controls.PageTemplates;
using MetroWPFTemplate.Metro.Dialogs;
using MetroWPFTemplate.Metro.Native;

namespace MetroWPFTemplate.Windows
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
            DwmDropShadow.DropShadowToWindow(this);
            AddHandler(CloseableTabItem.CloseTabEvent, new RoutedEventHandler(CloseTab));
            Settings.HomeWindow = this;

            UpdateTitleText("Home");
            UpdateStatusText("Ready...");

            //Window_StateChanged(null, null);
            ClearTabs();

            AddTabModule(TabGenre.StartPage);

            // Set width/height/state from last session
            if (!double.IsNaN(Settings.ApplicationSizeHeight))
                Height = Settings.ApplicationSizeHeight;
            if (!double.IsNaN(Settings.ApplicationSizeWidth))
                Width = Settings.ApplicationSizeWidth;
            WindowState = Settings.ApplicationSizeMaximize ? WindowState.Maximized : WindowState.Normal;

            Window_StateChanged(null, null);
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var handle = (new WindowInteropHelper(this)).Handle;
	        var hwndSource = HwndSource.FromHwnd(handle);
	        if (hwndSource != null) hwndSource.AddHook(WindowProc);
        }

        #region Public Access Modifiers
        /// <summary>
		/// Set the title text of Metro WPF Template
        /// </summary>
        /// <param name="title">Current Title, Metro WPF Template shall add the rest for you.</param>
        public void UpdateTitleText(string title)
        {
            this.Title = title.Trim() + " - Metro WPF Template";
			lblTitle.Text = title.Trim() + " - Metro WPF Template";
        }

        /// <summary>
		/// Set the status text of Metro WPF Template
        /// </summary>
        /// <param name="status">Current Status of Metro WPF Template</param>
        public void UpdateStatusText(string status)
        {
            this.Status.Text = status;

            _statusUpdateTimer.Stop();
            _statusUpdateTimer.Interval = new TimeSpan(0, 0, 0, 4);
            _statusUpdateTimer.Tick += statusUpdateCleaner_Clear;
            _statusUpdateTimer.Start();
        }
        private void statusUpdateCleaner_Clear(object sender, EventArgs e)
        {
            this.Status.Text = "Ready...";
        }
        private DispatcherTimer _statusUpdateTimer = new DispatcherTimer();
        #endregion
        #region Tab Manager
		public void ClearTabs()
		{
			homeTabControl.Items.Clear();
		}
        private void CloseTab(object source, RoutedEventArgs args)
        {
            TabItem tabItem = args.Source as TabItem;
            if (tabItem != null)
            {
                dynamic tabContent = tabItem.Content;

                if (tabContent.Close())
                {
                    TabControl tabControl = tabItem.Parent as TabControl;
                    if (tabControl != null)
                        tabControl.Items.Remove(tabItem);
                }
            }
        }

        public void ExternalTabClose(TabItem tab)
        {
            homeTabControl.Items.Remove(tab);

            foreach (TabItem datTab in homeTabControl.Items)
                if (datTab.Header.ToString() == "Start Page")
                {
                    homeTabControl.SelectedItem = datTab;
                    return;
                }

            if (homeTabControl.Items.Count > 0)
                homeTabControl.SelectedIndex = homeTabControl.Items.Count - 1;
        }
        public void ExternalTabClose(TabGenre tabGenre)
        {
            string tabHeader = "";
            if (tabGenre == TabGenre.StartPage)
                tabHeader = "Start Page";

            TabItem toRemove = null;
            foreach (TabItem tab in homeTabControl.Items)
                if (tab.Header.ToString() == tabHeader)
                    toRemove = tab;

            if (toRemove != null)
                homeTabControl.Items.Remove(toRemove);
        }

        public enum TabGenre
        {
            StartPage
        }
        public void AddTabModule(TabGenre tabG)
        {
            CloseableTabItem closeableTabItem = new CloseableTabItem();
            TabItem regularTabItem = new TabItem();
            closeableTabItem.Header = regularTabItem.Header = "swag";
            closeableTabItem.HorizontalAlignment = regularTabItem.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            closeableTabItem.VerticalAlignment = regularTabItem.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;


            switch (tabG)
            {
                case TabGenre.StartPage:
                    regularTabItem.Header = "Start Page";
                    regularTabItem.Content = new StartPage();
                    break;
            }

            if (closeableTabItem.Header != "swag")
            {
                foreach (TabItem tabb in homeTabControl.Items)
                    if (tabb.Header == closeableTabItem.Header)
                    {
                        homeTabControl.SelectedItem = tabb;
                        return;
                    }

                homeTabControl.Items.Add(closeableTabItem);
                homeTabControl.SelectedItem = closeableTabItem;
            }
            if (regularTabItem.Header != "swag")
            {
                foreach (TabItem tabb in homeTabControl.Items)
                    if (tabb.Header == regularTabItem.Header)
                    {
                        homeTabControl.SelectedItem = tabb;
                        return;
                    }

                homeTabControl.Items.Add(regularTabItem);
                homeTabControl.SelectedItem = regularTabItem;
            }
        }
        #endregion
        #region More WPF Annoyance
        private void headerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }

        private void ResizeDrop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double yadjust = this.Height + e.VerticalChange;
            double xadjust = this.Width + e.HorizontalChange;

            if (xadjust > this.MinWidth)
                this.Width = xadjust;
            if (yadjust > this.MinHeight)
                this.Height = yadjust;
        }
        private void ResizeRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double xadjust = this.Width + e.HorizontalChange;

            if (xadjust > this.MinWidth)
                this.Width = xadjust;
        }
        private void ResizeBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double yadjust = this.Height + e.VerticalChange;

            if (yadjust > this.MinHeight)
                this.Height = yadjust;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                borderFrame.BorderThickness = new Thickness(1, 1, 1, 23);
                Settings.ApplicationSizeMaximize = false;
                Settings.ApplicationSizeHeight = this.Height;
                Settings.ApplicationSizeWidth = this.Width;
                Settings.UpdateSettings();

                btnActionRestore.Visibility = System.Windows.Visibility.Collapsed;
                btnActionMaxamize.Visibility = ResizeDropVector.Visibility = ResizeDrop.Visibility = ResizeRight.Visibility = ResizeBottom.Visibility = System.Windows.Visibility.Visible;
            }
            else if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                borderFrame.BorderThickness = new Thickness(0, 0, 0, 23);
                Settings.ApplicationSizeMaximize = true;
                Settings.UpdateSettings();

                btnActionRestore.Visibility = System.Windows.Visibility.Visible;
                btnActionMaxamize.Visibility = ResizeDropVector.Visibility = ResizeDrop.Visibility = ResizeRight.Visibility = ResizeBottom.Visibility = System.Windows.Visibility.Collapsed;
            }
            /*
             * ResizeDropVector
             * ResizeDrop
             * ResizeRight
             * ResizeBottom
             */
        }
        private void headerThumb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
                this.WindowState = System.Windows.WindowState.Maximized;
            else if (this.WindowState == System.Windows.WindowState.Maximized)
                this.WindowState = System.Windows.WindowState.Normal;
        }
        private void btnActionSupport_Click(object sender, RoutedEventArgs e)
        {
            // Load support page?
        }
        private void btnActionMinimize_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        private void btnActionRestore_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;
        }
        private void btnActionMaxamize_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Maximized;
        }
        private void btnActionClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
        #region Maximize Workspace Workarounds
        private System.IntPtr WindowProc(
              System.IntPtr hwnd,
              int msg,
              System.IntPtr wParam,
              System.IntPtr lParam,
              ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (System.IntPtr)0;
        }
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            Monitor_Workarea.MINMAXINFO mmi = (Monitor_Workarea.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Monitor_Workarea.MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = Monitor_Workarea.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {
                System.Windows.Forms.Screen scrn = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);

                Monitor_Workarea.MONITORINFO monitorInfo = new Monitor_Workarea.MONITORINFO();
                Monitor_Workarea.GetMonitorInfo(monitor, monitorInfo);
                Monitor_Workarea.RECT rcWorkArea = monitorInfo.rcWork;
                Monitor_Workarea.RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);

                /*
                mmi.ptMaxPosition.x = Math.Abs(scrn.Bounds.Left - scrn.WorkingArea.Left);
                mmi.ptMaxPosition.y = Math.Abs(scrn.Bounds.Top - scrn.WorkingArea.Top);
                mmi.ptMaxSize.x = Math.Abs(scrn.Bounds.Right - scrn.WorkingArea.Left);
                mmi.ptMaxSize.y = Math.Abs(scrn.Bounds.Bottom - scrn.WorkingArea.Top);
                */
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
        #endregion
        #region Opacity Masking
        public int OpacityIndex = 0;
        public void ShowMask()
        {
            OpacityIndex++;
            OpacityMask.Visibility = System.Windows.Visibility.Visible;
        }
        public void HideMask()
        {
            OpacityIndex--;

            if (OpacityIndex == 0)
                OpacityMask.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        private void homeTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = (TabItem)homeTabControl.SelectedItem;

			// yolo
        }

        private void menuCloseApplication_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
    }
}