using System;

namespace WpfApp4.Entities
{
    public class Consultation
    {
        public int ID { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }



        // Check if consultation times overlap for a given day
        public bool Overlap(DateTime dateTime)
        {
            return dateTime.DayOfWeek == Day && dateTime.TimeOfDay >= Start && dateTime.TimeOfDay < End;
        }

        public override string ToString()
        {
            return Day + ", Starts: " + Start + ", Ends: " + End;
        }

    }
}
