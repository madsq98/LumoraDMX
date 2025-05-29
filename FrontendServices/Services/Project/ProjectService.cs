using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FrontendServices.Services.Project
{
    public class ProjectService : ServiceBase
    {
        private ProjectType? _selectedProject = null;
        public event Action? ProjectLoaded;
        public async Task<List<ProjectType>> GetProjects()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "project");
            var response = await SendAsync(request);
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong!");
            }

            return await response.Content.ReadFromJsonAsync<List<ProjectType>>();
        }

        public void LoadProject(ProjectType project) {
            _selectedProject = project;
            ProjectLoaded?.Invoke();
        }

        public bool IsProjectLoaded() => _selectedProject != null;

        public ProjectType? GetLoadedProject() => _selectedProject;
    }

    public class ProjectType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public List<FixtureType> Fixtures { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class FixtureType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartAddress { get; set; }
        public FixtureTemplate FixtureTemplate { get; set; }
    }

    public class FixtureTemplate
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Mode { get; set; }
        public Dictionary<string, int> Channels { get; set; }
    }
}
