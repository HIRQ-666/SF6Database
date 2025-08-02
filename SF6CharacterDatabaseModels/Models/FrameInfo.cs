namespace SF6CharacterDatabaseModels.Models
{
    public class FrameInfo
    {
        public int Start { get; set; } = -999;
        public int Active { get; set; } = -999;
        public int Stiffness { get; set; } = -999;
        public int All { get; set; } = -999;

        public FrameInfo(int start = -999, int active = -999, int stiffness = -999, int all = -999)
        {
            Start = start;
            Active = active;
            Stiffness = stiffness;
            All = all;
        }
    }
}
