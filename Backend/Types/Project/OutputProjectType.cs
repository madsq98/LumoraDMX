using Backend.Types.Fixture;
using Infrastructure.Entities;

namespace Backend.Types.Project
{
    public class OutputProjectType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public List<OutputFixtureType> Fixtures { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
