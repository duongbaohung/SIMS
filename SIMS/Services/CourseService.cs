using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;

        public CourseService(ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepo.GetAllCoursesAsync();
        }

        public async Task<bool> AddCourseAsync(Course course)
        {
            // Apply business rules before saving
            course.Status = 1; // 1 = Open
            course.Enrolled = 0; // Starts with 0 students
            course.CreatedAt = DateTime.Now;

            return await _courseRepo.AddCourseAsync(course);
        }
        /// <summary>
        /// Implements the enrollment logic by validating inputs and 
        /// calling the repository layer.
        /// </summary>
        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            // Basic validation: ensure IDs are valid before proceeding
            if (studentId <= 0 || courseId <= 0)
            {
                return false;
            }

            // Coordination with the repository to persist the enrollment record
            return await _courseRepo.EnrollStudentAsync(studentId, courseId);
        }


    }
}
