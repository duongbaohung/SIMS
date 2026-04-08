using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their unique username.
        /// </summary>
        Task<User?> GetUserByUsername(string username);

        /// <summary>
        /// Retrieves a user by their primary key.
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Retrieves all users matching a specific role.
        /// </summary>
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        Task<bool> AddUserAsync(User user);

        /// <summary>
        /// Updates an existing user using the SetValues pattern.
        /// </summary>
        Task<bool> UpdateUserAsync(User user);

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}