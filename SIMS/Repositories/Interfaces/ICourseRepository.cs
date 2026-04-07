using SIMS.DatabaseContext.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<bool> AddCourseAsync(Course course);
        /// <summary>
        /// Saves a new enrollment record to the database.
        /// </summary>
        Task<bool> EnrollStudentAsync(int studentId, int courseId);

    }
}