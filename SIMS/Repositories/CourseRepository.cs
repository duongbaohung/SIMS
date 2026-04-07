using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext;
using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<bool> AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Persists a new enrollment and updates the course enrollment counter.
        /// </summary>
        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {
            // 1. Create the enrollment record
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now
            };

            // CRITICAL: Ensure 'public DbSet<Enrollment> Enrollments { get; set; }' 
            // is defined in your SimsDbContext.cs file to resolve red lines here.
            await _context.Enrollments.AddAsync(enrollment);

            // 2. Find the course to update the 'Enrolled' count
            var course = await _context.Courses.FindAsync(courseId);
            if (course != null)
            {
                // Ensure your Course entity has the 'Enrolled' property defined.
                course.Enrolled += 1;
            }

            // 3. Save both changes (Enrollment insert + Course update) in one transaction
            return await _context.SaveChangesAsync() > 0;
        }
    }
}