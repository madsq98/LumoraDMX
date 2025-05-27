namespace Backend.Types.SimpleDmx
{
    public class AnimateChannelType
    {
        public int Channel { get; set; }
        public int StartValue { get; set; }
        public int TargetValue { get; set; }
        public int Duration { get; set; }
        public int Cycles { get; set; }
    }
}
