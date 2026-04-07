using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.DatabaseContext.Entities
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Navigation properties (Optional, helps with joins later)
        [ForeignKey("StudentId")]
        public virtual User? Student { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }
    }
}