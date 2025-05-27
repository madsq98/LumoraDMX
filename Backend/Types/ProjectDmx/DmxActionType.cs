namespace Backend.Types.ProjectDmx
{
    public class DmxActionType
    {
        public int FixtureId { get; set; }
        public string Channel { get; set; }
        public string Action { get; set; }
        public int TargetValue { get; set; }
        public int? StartValue { get; set; }
        public int? Duration { get; set; }
        public int? Cycles { get; set; }
    }
}
