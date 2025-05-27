namespace Backend.Types.Fixture
{
    public class OutputFixtureType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartAddress { get; set; }
        public FixtureTemplate FixtureTemplate { get; set; }
    }
}
