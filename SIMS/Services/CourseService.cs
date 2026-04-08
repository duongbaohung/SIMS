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

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _courseRepo.GetCourseByIdAsync(id);
        }

        public async Task<Course> GetCourseByCodeAsync(string courseCode)
        {
            if (string.IsNullOrWhiteSpace(courseCode)) return null;
            return await _courseRepo.GetCourseByCodeAsync(courseCode);
        }

        public async Task<bool> AddCourseAsync(Course course)
        {
            // Apply business rules before saving
            course.Status = 1; // 1 = Open
            course.Enrolled = 0; // Starts with 0 students
            course.CreatedAt = DateTime.Now;

            return await _courseRepo.AddCourseAsync(course);
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            // Apply business logic during update: ensure status reflects enrollment/capacity
            if (course.Enrolled >= course.Capacity)
            {
                course.Status = 0; // Close if full
            }
            else
            {
                course.Status = 1; // Keep open if space remains
            }

            return await _courseRepo.UpdateCourseAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            if (id <= 0) return false;
            return await _courseRepo.DeleteCourseAsync(id);
        }

        /// <summary>
        /// Implements the enrollment logic by validating inputs and 
        /// calling the repository layer.
        /// </summary>
        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            // 1. Basic validation: ensure IDs are valid
            if (studentId <= 0 || courseId <= 0)
            {
                return false;
            }

            // 2. Business Rule Check: Ensure course exists and has capacity
            var course = await _courseRepo.GetCourseByIdAsync(courseId);
            if (course == null || course.Status == 0 || course.Enrolled >= course.Capacity)
            {
                return false;
            }

            // 3. Coordination with the repository to persist the enrollment record
            return await _courseRepo.EnrollStudentAsync(studentId, courseId);
        }
    }
}