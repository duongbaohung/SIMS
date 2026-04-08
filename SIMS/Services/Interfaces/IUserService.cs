using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user based on username and password.
        /// </summary>
        Task<User?> LoginUserAsync(string username, string password);

        /// <summary>
        /// Retrieves all users with the "Student" role.
        /// </summary>
        Task<IEnumerable<User>> GetAllStudentsAsync();

        /// <summary>
        /// Retrieves a specific student by their primary key ID.
        /// </summary>
        Task<User?> GetStudentByIdAsync(int id);

        /// <summary>
        /// Retrieves a user specifically by their Username for duplicate checking.
        /// </summary>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Retrieves a user specifically by their Email for duplicate checking.
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Registers a new student with default business rules (Role, Status, Password).
        /// </summary>
        Task<bool> AddStudentAsync(User student);

        /// <summary>
        /// Updates an existing student's profile information.
        /// </summary>
        Task<bool> UpdateStudentAsync(User student);

        /// <summary>
        /// Removes a student record from the system.
        /// </summary>
        Task<bool> DeleteStudentAsync(int id);
    }
}