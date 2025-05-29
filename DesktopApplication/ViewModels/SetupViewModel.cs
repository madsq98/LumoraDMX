using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using FrontendServices.Services.Project;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace DesktopApplication.ViewModels
{
    public partial class SetupViewModel : ObservableObject
    {
        private readonly ProjectService _projectService;
        [ObservableProperty]
        private ObservableCollection<ProjectType> _projects = new();
        [ObservableProperty]
        private ProjectType? _selectedProject;
        public SetupViewModel(ProjectService projectService) 
        {
            _projectService = projectService;
            _ = LoadProjects();
        }

        private async Task LoadProjects()
        {
            var loadedProjects = await _projectService.GetProjects();
            Projects = new ObservableCollection<ProjectType>(loadedProjects);
        }

        [RelayCommand(CanExecute = nameof(CanLoadProject))]
        private void LoadProject()
        {
            if (_selectedProject is not null)
            {
                _projectService.LoadProject(_selectedProject);
            }
        }

        private bool CanLoadProject() => _selectedProject is not null;

        partial void OnSelectedProjectChanged(ProjectType? value)
        {
            LoadProjectCommand.NotifyCanExecuteChanged();
        }
    }
}
