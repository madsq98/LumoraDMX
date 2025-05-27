namespace Backend.Types.Fixture
{
    public class FixtureTemplate
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Mode { get; set; }
        public Dictionary<string, int> Channels { get; set; }
    }
}
