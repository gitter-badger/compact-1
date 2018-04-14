using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO;

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
                    foreach (CheckBox checkBox in canvas.Children)
                    {
                        if (checkBox.IsChecked.Value == true)
                        {
                            SoftwareListItem queueItem = (SoftwareListItem)checkBox.Tag;
                            SoftwareListQueue.Add(queueItem);
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
                string dataFolder = Utilities.GetDataFolder();
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri(item.Url), dataFolder);
            }

            // Clear the software queue and enable user input
            SoftwareListQueue.Clear();
            IsEnabled = true;
        }
    }
}
