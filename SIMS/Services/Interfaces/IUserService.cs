using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Services.Interfaces
{
    public interface IUserService
    {
        /// Authenticates a user based on username and password.
        Task<User?> LoginUserAsync(string username, string password);

        /// Retrieves all users with the "Student" role.
        Task<IEnumerable<User>> GetAllStudentsAsync();

        /// Retrieves a specific student by their primary key ID.
        Task<User?> GetStudentByIdAsync(int id);

        /// Registers a new student with default business rules (Role, Status, Password).
        Task<bool> AddStudentAsync(User student);

        /// Updates an existing student's profile information.
        Task<bool> UpdateStudentAsync(User student);

        /// Removes a student record from the system.
        Task<bool> DeleteStudentAsync(int id);
    }
}