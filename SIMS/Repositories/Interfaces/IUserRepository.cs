using SIMS.DatabaseContext.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);

        /// <summary>
        /// Retrieves a user by their unique Email address.
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}