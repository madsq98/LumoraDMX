using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;

namespace DesktopApplication.Views
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
            Background = Brushes.Transparent;

            InitializeComponent();
        }
    }
}
