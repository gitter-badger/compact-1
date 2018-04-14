using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public bool Closeable { get; set; } = true;
        public SoftwareListItem[] SoftwareList { get; set; }
        public List<SoftwareListItem> SoftwareListQueue { get; set; } = new List<SoftwareListItem>();

        private async void OnClickInstall(object sender, RoutedEventArgs e)
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
            Closeable = false;
            
            // Download all installers
            var progress = new Progress<BundleProgress>();
            progress.ProgressChanged += (o, ev) =>
            {
                progressBar.Value = ev.ProgressPercentage;
                lblStatus.Content = ev.ProgressPercentage + "% downloaded.";
            };

            await Utilities.DownloadInstallersAsync(SoftwareListQueue.ToArray(), progress);

            // Clear the software queue and progress values
            SoftwareListQueue.Clear();
            progressBar.Value = 0;

            // Install all downloaded software
            try
            {
                List<string> installerFiles = new List<string>();
                installerFiles.AddRange(Directory.GetFiles(Utilities.GetTempFolder()));
                foreach (string item in installerFiles)
                {
                    lblStatus.Content = "Installing " + Path.GetFileName(item) + "...";
                    Process process = new Process()
                    {
                        StartInfo = new ProcessStartInfo(item)
                    };

                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception)
            {
            }

            lblStatus.Content = "Cleaning up temporary files...";
            foreach (string item in Directory.GetFiles(Utilities.GetTempFolder()))
            {
                File.Delete(item);
            }

            MessageBox.Show("This bundle has been installed successfully on your computer!",
                "Successfully installed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            IsEnabled = true;
            Closeable = true;
            Close();
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
                Close();
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Closeable;
        }
    }
}
