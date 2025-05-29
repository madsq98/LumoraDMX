using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DesktopApplication.Views
{
    public partial class SetupView : UserControl
    {
        public SetupView()
        {
            InitializeComponent();
        }

        private void ToggleCreateProjectPanel()
        {
            bool shouldShow = !CreateProjectPanel.IsVisible;

            CreateProjectPanel.IsVisible = shouldShow;

            MainGrid.ColumnDefinitions[1].Width = shouldShow
                ? new GridLength(700)
                : new GridLength(0);
        }

        private void OnCreateProjectClick(object? sender, RoutedEventArgs e)
        {
            ToggleCreateProjectPanel();
        }
    }
}
