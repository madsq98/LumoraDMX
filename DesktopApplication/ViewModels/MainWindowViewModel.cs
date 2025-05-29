using System;
using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontendServices.Services.Project;
using FrontendServices.Services.ProjectDmx;
using FrontendServices.Services.SimpleDmx;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopApplication.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public object? currentView;

        [ObservableProperty]
        public IBrush currentTabColor = Brushes.White;
        [ObservableProperty]
        private bool _isProjectLoaded = false;

        public IRelayCommand ShowSetupCommand { get; }
        public IRelayCommand ShowSimpleDmxCommand { get; }
        public IRelayCommand ShowEditCommand { get; }
        public IRelayCommand ShowPresetsCommand { get; }

        private readonly ProjectService _projectService;
        private readonly SimpleDmxService _simpleDmxService;
        private readonly ProjectDmxService _projectDmxService;

        private readonly object _setupVM;
        private readonly object _simpleDmxVM;
        private readonly object _editVM;
        private readonly object _presetsVM;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetRequiredService<ProjectService>();
            _simpleDmxService = serviceProvider.GetRequiredService<SimpleDmxService>();
            _projectDmxService = serviceProvider.GetRequiredService<ProjectDmxService>();

            _setupVM = new SetupViewModel(_projectService);
            _simpleDmxVM = new SimpleDmxViewModel();
            _editVM = new EditViewModel();
            _presetsVM = new PresetsViewModel();

            ShowSetupCommand = new RelayCommand(() =>
            {
                CurrentView = _setupVM;
                CurrentTabColor = GetBrush("SetupColor");
            });

            ShowSimpleDmxCommand = new RelayCommand(() =>
            {
                CurrentView = _simpleDmxVM;
                CurrentTabColor = GetBrush("SimpleDmxColor");
            });

            ShowEditCommand = new RelayCommand(() =>
            {
                CurrentView = _editVM;
                CurrentTabColor = GetBrush("EditColor");
            });

            ShowPresetsCommand = new RelayCommand(() =>
            {
                CurrentView = _presetsVM;
                CurrentTabColor = GetBrush("PresetsColor");
            });

            // Set initial view and color
            CurrentView = _setupVM;
            CurrentTabColor = GetBrush("SetupColor");

            _projectService.ProjectLoaded += () =>
            {
                IsProjectLoaded = _projectService.IsProjectLoaded();
            };
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
