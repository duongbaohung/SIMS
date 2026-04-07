using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        Task<bool> AddCourseAsync(Course course);
        /// <summary>
        /// Enrolls a student in a course by coordinating with the repository.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
        Task<bool> EnrollStudentAsync(int studentId, int courseId);

    }
}