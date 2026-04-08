using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Services.Interfaces
{
    public interface ICourseService
    {
        /// <summary>
        /// Retrieves all courses from the database.
        /// </summary>
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        /// <summary>
        /// Retrieves a single course by its primary key.
        /// </summary>
        Task<Course> GetCourseByIdAsync(int id);

        /// <summary>
        /// Retrieves a single course by its unique CourseCode.
        /// </summary>
        Task<Course> GetCourseByCodeAsync(string courseCode);

        /// <summary>
        /// Adds a new course to the database with initial business rules applied.
        /// </summary>
        Task<bool> AddCourseAsync(Course course);

        /// <summary>
        /// Updates an existing course's details and adjusts status based on capacity.
        /// </summary>
        Task<bool> UpdateCourseAsync(Course course);

        /// <summary>
        /// Deletes a course by its primary key ID.
        /// </summary>
        Task<bool> DeleteCourseAsync(int id);

        /// <summary>
        /// Enrolls a student in a course by coordinating with the repository.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
    }
}