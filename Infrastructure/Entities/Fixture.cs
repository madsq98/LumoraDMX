namespace Infrastructure.Entities
{
    public class Fixture
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public int StartAddress { get; set; }
        public string FixtureTemplate { get; set; }
    }
}
