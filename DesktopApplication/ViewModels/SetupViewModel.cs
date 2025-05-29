using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FrontendServices.Services.Project;

namespace DesktopApplication.ViewModels
{
    public partial class SetupViewModel : ObservableObject
    {
        private readonly ProjectService _projectService;
        public string TestValue => _projectService.CurrentProjectName();
        public SetupViewModel(ProjectService projectService) 
        {
            _projectService = projectService;
        }
    }
}
