using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace Compact
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Canvas CreateBundleEntry(BundleEntry entry)
        {
            Canvas mainCanvas = new Canvas()
            {
                Width = 350,
                Height = 90,
                Background = new ImageBrush((ImageSource) new ImageSourceConverter()
                    .ConvertFromString(entry.BackgroundImage.AbsoluteUri)),
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 20,
                    RenderingBias = RenderingBias.Performance,
                    Opacity = 0.25
                }
            };

            TextBlock nameLabel = new TextBlock()
            {                
                Text = entry.Name,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Width = 320,
                Height = 90,
                Foreground = entry.ForegroundColor,
                FontFamily = new FontFamily("Segoe UI Semilight"),
                FontSize = 24.0
            };

            mainCanvas.Margin = new Thickness(15);
            mainCanvas.Children.Add(nameLabel);
            return mainCanvas;
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            // TO-DO: Load packages from JSON file
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
