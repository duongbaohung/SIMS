using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext;
using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;

namespace SIMS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SimsDbContext _context;
        public UserRepository(SimsDbContext simsDb)
        {
            _context = simsDb;
        }
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id).AsTask();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
