using SIMS.DatabaseContext.Entities;

namespace SIMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(int id);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<bool> AddUserAsync(User user);
    }
}
