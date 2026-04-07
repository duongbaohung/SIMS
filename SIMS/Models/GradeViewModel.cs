using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models
{
    public class GradeViewModel
    {
        [Required(ErrorMessage = "Please select a student")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please enter a score")]
        [Range(0, 100)]
        public decimal Score { get; set; }

        public string? Remarks { get; set; }

        // Dropdown data
        public List<StudentItem>? Students { get; set; }
        public List<CourseItem>? Courses { get; set; }
    }
}