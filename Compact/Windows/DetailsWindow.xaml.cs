using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Compact.Windows
{
    public partial class DetailsWindow : Window
    {
        public DetailsWindow()
        {
            InitializeComponent();
        }

        public SoftwareListItem[] SoftwareList { get; set; }
        public List<SoftwareListItem> SoftwareListQueue { get; set; } = new List<SoftwareListItem>();

        private void OnClickInstall(object sender, RoutedEventArgs e)
        {
            // Iterates through all selected values
            foreach (Canvas canvas in lstSoftware.Items)
            {
                try
                {
                    foreach (object obj in canvas.Children)
                    {
                        if(obj.GetType() == typeof(CheckBox))
                        {
                            CheckBox checkBox = (CheckBox)obj;
                            if (checkBox.IsChecked.Value == true)
                            {
                                SoftwareListItem queueItem = (SoftwareListItem)checkBox.Tag;
                                SoftwareListQueue.Add(queueItem);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }          
            }

            // Disable all user input
            IsEnabled = false;

            // Download all installers
            foreach (SoftwareListItem item in SoftwareListQueue)
            {
                Utilities.DownloadInstallerAsync(item);
            }

            // Clear the software queue and enable user input
            SoftwareListQueue.Clear();
            IsEnabled = true;
        }

        private enum SelectionState
        {
            Checked,
            Unchecked
        }

        private void SetGeneralSelectionState(ListBox softwareList, SelectionState state)
        {
            foreach (Canvas canvas in softwareList.Items)
            {
                try
                {
                    foreach (object obj in canvas.Children)
                    {
                        if(obj.GetType() == typeof(CheckBox))
                        {
                            CheckBox checkBox = (CheckBox)obj;
                            if (state == SelectionState.Checked)
                            {
                                checkBox.IsChecked = true;
                            }
                            else
                            {
                                checkBox.IsChecked = false;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnClickSelectAll(object sender, RoutedEventArgs e)
        {
            SetGeneralSelectionState(lstSoftware, SelectionState.Checked);
        }

        private void OnClickDeselectAll(object sender, RoutedEventArgs e)
        {
            SetGeneralSelectionState(lstSoftware, SelectionState.Unchecked);
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
