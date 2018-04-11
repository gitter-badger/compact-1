using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Compact
{
    public class BundleEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string Icon { get; set; }
        public Uri BackgroundImage { get; set; }
        public SolidColorBrush ForegroundColor { get; set; }
    }
}
