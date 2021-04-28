namespace EdlightMobileClient.Models
{
    public class Schedule
    {
        public string DayWeek { get; set; }
        public TimeLessons TimeLesson { get; set; }
        public bool EvenNumberedWeek { get; set; }
        public string AcademicDescipline { get; set; }
        public string Teacher { get; set; }
        public string TypeClass { get; set; }
        public string Audience { get; set; }
    }
}
