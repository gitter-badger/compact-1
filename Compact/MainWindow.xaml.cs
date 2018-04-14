using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Net;
using Newtonsoft.Json;

namespace Compact
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string GetRawJson(string jsonUrl)
        {
            try
            {
                // TO-DO: Seperate threads
                WebClient webClient = new WebClient();
                string rawJson = webClient.DownloadString(jsonUrl);
                return rawJson;
            }
            catch (Exception)
            {
                // TO-DO: Show error dialog
                return null;
            }
        }

        private Canvas CreateBundleEntry(Bundle bundleEntry)
        {
            Canvas mainCanvas = new Canvas()
            {
                Width = 350,
                Height = 90,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 20,
                    RenderingBias = RenderingBias.Performance,
                    Opacity = 0.25
                }
            };

            try
            {
                mainCanvas.Background = new ImageBrush((ImageSource)new ImageSourceConverter()
                    .ConvertFromString(bundleEntry.BannerUrl));
            }
            catch (Exception)
            {
                mainCanvas.Background = new SolidColorBrush(Color.FromRgb(38, 38, 38));
            }

            mainCanvas.Margin = new Thickness(15);
            return mainCanvas;
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            string rawJson = GetRawJson("https://raw.githubusercontent.com/DeadNetOfficial/compact/master/bundles.json");
            Bundle[] bundles = JsonConvert.DeserializeObject<Bundle[]>(rawJson);
            foreach (Bundle bundle in bundles)
            {
                Canvas canvas = CreateBundleEntry(new Bundle()
                {
                    Name = bundle.Name,
                    Description = bundle.Description,
                    BannerUrl = bundle.BannerUrl,
                    SoftwareList = bundle.SoftwareList
                });

                canvas.MouseLeftButtonDown += (o, ev) =>
                {
                    var detailsWindow = new Windows.DetailsWindow();
                    try
                    {
                        detailsWindow.bannerCanvas.Background = new ImageBrush((ImageSource)new ImageSourceConverter()
                                        .ConvertFromString(bundle.BannerUrl));
                    }
                    catch (Exception)
                    {
                        detailsWindow.bannerCanvas.Background = new SolidColorBrush(Color.FromRgb(38, 38, 38));
                    }

                    foreach (SoftwareListItem item in bundle.SoftwareList)
                    {
                        Canvas itemCanvas = new Canvas()
                        {
                            Height = 30,
                            ToolTip = item.Description
                        };

                        itemCanvas.Children.Add(new CheckBox() { Margin = new Thickness(0, 7.5, 0, 0), IsChecked = true, Tag = item });
                        itemCanvas.Children.Add(new Label() { Margin = new Thickness(17, 2, 0, 0), Content = item.Name });
                        detailsWindow.lstSoftware.Items.Add(itemCanvas);
                        detailsWindow.SoftwareList = bundle.SoftwareList.ToArray();
                        detailsWindow.txtDescription.Text = bundle.Description;
                        detailsWindow.Title = "Bundle Details: " + bundle.Name + " Bundle";
                    }

                    detailsWindow.ShowDialog();
                };

                bundlePanel.Children.Add(canvas);
            }            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
