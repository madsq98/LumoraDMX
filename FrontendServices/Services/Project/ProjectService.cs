using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendServices.Services.Project
{
    public class ProjectService
    {
        private string? _backend = null;
        public ProjectService() { }

        public void SetBackend(string backend)
        {
            _backend = backend;
            Console.WriteLine($"SET BACKEND TO {backend}");
        }

        public string CurrentProjectName()
        {
            return "test project";
        }
    }
}
