using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DesktopApplication.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public object? currentView;

        [ObservableProperty]
        public IBrush currentTabColor = Brushes.White;

        public IRelayCommand ShowSetupCommand { get; }
        public IRelayCommand ShowSimpleDmxCommand { get; }
        public IRelayCommand ShowEditCommand { get; }
        public IRelayCommand ShowPresetsCommand { get; }

        public MainWindowViewModel()
        {
            ShowSetupCommand = new RelayCommand(() =>
            {
                CurrentView = new SetupViewModel();
                CurrentTabColor = GetBrush("SetupColor");
            });

            ShowSimpleDmxCommand = new RelayCommand(() =>
            {
                CurrentView = new SimpleDmxViewModel();
                CurrentTabColor = GetBrush("SimpleDmxColor");
            });

            ShowEditCommand = new RelayCommand(() =>
            {
                CurrentView = new EditViewModel();
                CurrentTabColor = GetBrush("EditColor");
            });

            ShowPresetsCommand = new RelayCommand(() =>
            {
                CurrentView = new PresetsViewModel();
                CurrentTabColor = GetBrush("PresetsColor");
            });

            // Set initial view and color
            CurrentView = new SetupViewModel();
            CurrentTabColor = GetBrush("SetupColor");
        }

        private IBrush GetBrush(string resourceKey)
        {
            if (Application.Current?.Resources.TryGetValue(resourceKey, out var value) == true &&
                value is IBrush brush)
            {
                return brush;
            }

            return Brushes.White;
        }
    }
}
