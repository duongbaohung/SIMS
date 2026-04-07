using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models
{
    /// <summary>
    /// ViewModel used for the process of enrolling a student in a course.
    /// </summary>
    public class EnrollmentViewModel
    {
        [Required(ErrorMessage = "Please select a student")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        // These lists will be used to populate the dropdown menus in the View
        public List<StudentItem>? Students { get; set; }
        public List<CourseItem>? Courses { get; set; }
    }

    /// <summary>
    /// Simple DTO to represent a student in a dropdown list.
    /// </summary>
    public class StudentItem
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Simple DTO to represent a course in a dropdown list.
    /// </summary>
    public class CourseItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}