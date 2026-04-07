namespace SIMS.DatabaseContext.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = null!; // e.g., "CS101"
        public string CourseName { get; set; } = null!; // e.g., "Introduction to Programming"
        public string? Description { get; set; }
        public int Credits { get; set; }
        public int Capacity { get; set; }
        public int Enrolled { get; set; } = 0;
        public byte Status { get; set; } = 1; // 1 = Active/Open, 0 = Closed
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}