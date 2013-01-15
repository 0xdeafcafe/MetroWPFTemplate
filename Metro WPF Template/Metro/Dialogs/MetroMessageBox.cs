﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroWPFTemplate.Backend;

namespace MetroWPFTemplate.Metro.Dialogs
{
    public class MetroMessageBox
    {
        /// <summary>
        /// Show a Metro Message Box
        /// </summary>
        /// <param name="title">The title of the Message Box</param>
        /// <param name="message">The message to be displayed in the Message Box</param>
        public static void Show(string title, string message)
        {
            Settings.HomeWindow.ShowMask();
            ControlDialogs.MessageBox msgBox = new ControlDialogs.MessageBox(title, message);
            msgBox.Owner = Settings.HomeWindow;
            msgBox.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            msgBox.ShowDialog();
            Settings.HomeWindow.HideMask();
        }
        /// <summary>
        /// Show a Metro Message Box
        /// </summary>
        /// <param name="message">The message to be displayed in the Message Box</param>
        public static void Show(string message)
        {
            Settings.HomeWindow.ShowMask();
            ControlDialogs.MessageBox msgBox = new ControlDialogs.MessageBox("Message Box", message);
            msgBox.Owner = Backend.Settings.HomeWindow;
            msgBox.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            msgBox.ShowDialog();
            Settings.HomeWindow.HideMask();
        }

        /// <summary>
        /// The Results a Metro MessageBox can return
        /// </summary>
        public enum MessageBoxResults
        {
            OK,
            Yes,
            No,
            Cancel
        }
        /// <summary>
        /// The diffrent Button Combinations that Metro Message Box's support
        /// </summary>
        public enum MessageBoxButtons
        {
            OK,
            OKCancel,
            YesNo,
            YesNoCancel
        }
        /// <summary>
        /// Show a Metro Message Box
        /// </summary>
        /// <param name="title">The title of the Message Box</param>
        /// <param name="message">The message to be displayed in the Message Box</param>
        /// <param name="buttons">The buttons to show in the Message Box</param>
        /// <returns>The button the user clicked</returns>
        public static MessageBoxResults Show(string title, string message, MessageBoxButtons buttons)
        {
            Settings.HomeWindow.ShowMask();
            ControlDialogs.MessageBoxOptions msgBox = new ControlDialogs.MessageBoxOptions(title, message, buttons);
            msgBox.Owner = Backend.Settings.HomeWindow;
            msgBox.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            msgBox.ShowDialog();
            Settings.HomeWindow.HideMask();

            return Backend.TempStorage.MessageBoxButtonStorage;
        }
    }
}