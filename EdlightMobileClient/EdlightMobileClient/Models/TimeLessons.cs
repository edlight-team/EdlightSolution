using System;

namespace EdlightMobileClient.Models
{
    public class TimeLessons
    {
        public int NumberClass { get; set; }

        public DateTime StartClass { get; set; }

        public DateTime StartBreak { get; set; }

        public DateTime EndBreak { get; set; }

        public DateTime EndClass { get; set; }
    }
}
