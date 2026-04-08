using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext;
using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMS.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SimsDbContext _context;

        public CourseRepository(SimsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Course> GetCourseByCodeAsync(string courseCode)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }

        public async Task<bool> AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            // Robust Update: Find the existing entity first to ensure ID tracking is correct
            var existing = await _context.Courses.FindAsync(course.Id);
            if (existing == null) return false;

            // Apply values from the posted model to the tracked entity
            _context.Entry(existing).CurrentValues.SetValues(course);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now
            };

            await _context.Enrollments.AddAsync(enrollment);

            var course = await _context.Courses.FindAsync(courseId);
            if (course != null)
            {
                course.Enrolled += 1;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        // --- NEW METHOD ---
        public async Task<IEnumerable<Enrollment>> GetRecentEnrollmentsAsync(int limit)
        {
            return await _context.Enrollments
                .OrderByDescending(e => e.EnrollmentDate)
                .Take(limit)
                .ToListAsync();
        }
    }
}