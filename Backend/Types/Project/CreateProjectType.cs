namespace Backend.Types.Project
{
    public class CreateProjectType
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public static explicit operator Infrastructure.Entities.Project(CreateProjectType createProjectType)
        {
            return new Infrastructure.Entities.Project
            {
                Title = createProjectType.Title,
                Description = createProjectType.Description,
                Author = createProjectType.Author,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };
        }
    }
}
