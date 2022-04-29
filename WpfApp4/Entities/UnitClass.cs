using System;

namespace WpfApp4.Entities
{
    public enum ClassType
    {
        Lecture,
        Tutorial,
        Practical,
        Workshop
    }
    public class UnitClass
    {
        public string UnitCode { get; set; }
        public Campus Campus { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public ClassType Type { get; set; }
        public string Room { get; set; }
        public int Staff { get; set; }


        // Check if class times overlap for a given day
        public bool Overlap(DateTime dateTime)
        {
            return dateTime.DayOfWeek == Day && dateTime.TimeOfDay >= Start && dateTime.TimeOfDay < End;
        }

        public override string ToString()
        {
            return UnitCode + ", " + Campus + " campus, " + Day + " " + Start + " until " + End + ", " + Type + " in room " + Room;
        }
    }
}
