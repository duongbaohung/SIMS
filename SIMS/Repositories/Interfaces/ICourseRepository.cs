using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        /// <summary>
        /// Retrieves a single course by its primary key.
        /// </summary>
        Task<Course> GetCourseByIdAsync(int id);

        /// <summary>
        /// Retrieves a single course by its unique CourseCode.
        /// </summary>
        Task<Course> GetCourseByCodeAsync(string courseCode);

        Task<bool> AddCourseAsync(Course course);

        /// <summary>
        /// Updates an existing course entity in the database.
        /// </summary>
        Task<bool> UpdateCourseAsync(Course course);

        /// <summary>
        /// Deletes a course from the database by its primary key.
        /// </summary>
        Task<bool> DeleteCourseAsync(int id);

        /// <summary>
        /// Saves a new enrollment record to the database and updates course enrollment count.
        /// </summary>
        Task<bool> EnrollStudentAsync(int studentId, int courseId);

        // --- NEW METHOD ADDED FOR RECENT ASSIGNMENTS TABLE ---
        /// <summary>
        /// Retrieves the most recent enrollment records.
        /// </summary>
        Task<IEnumerable<Enrollment>> GetRecentEnrollmentsAsync(int limit);
    }
}