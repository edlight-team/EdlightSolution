using EdlightMobileClient.Collections;
using System;
using System.Collections.Generic;

namespace EdlightMobileClient.Models
{
    public class DayModel : IIndexable
    {
        public DateTime Date { get; set; }
        public DateTime StartClasses { get; set; }
        public DateTime EndClasses { get; set; }
        public List<Schedule> Schedule { get; set; }
        public int Index { get; set; }
    }
}
