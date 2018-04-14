using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        public static async void DownloadInstallerAsync(SoftwareListItem softwareListItem)
        {
            WebClient webClient = new WebClient();
            await Task.Run(() =>
            {
                webClient.DownloadFile(new Uri(softwareListItem.Url), Path.Combine(GetTempFolder(), softwareListItem.FileName));
            });
        }
    }
}
