using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Compact
{
    public class Utilities
    {
        public static string GetDataFolder()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Compact");

            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            catch (Exception)
            {
            }

            return folderPath;
        }

        public static string GetTempFolder()
        {
            string folderPath = Path.Combine(GetDataFolder(), "Temp");

            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            catch (Exception)
            {
            }

            return folderPath;
        }

        public static async Task DownloadInstallersAsync(SoftwareListItem[] softwareItems, IProgress<BundleProgress> progress)
        {
            WebClient webClient = new WebClient();
            await Task.Run(() =>
            {
                int completeCount = 0;
                foreach (SoftwareListItem item in softwareItems)
                {
                    webClient.DownloadFile(new Uri(item.Url), Path.Combine(GetTempFolder(), item.FileName));
                    completeCount++;
                    progress.Report(new BundleProgress()
                    {
                        ProgressPercentage = (int)Math.Round((double)(100 * completeCount) / softwareItems.Length),
                        CurrentItem = item
                    });
                }
            });
        }
    }
}
