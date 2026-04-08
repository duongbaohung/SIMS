using Microsoft.EntityFrameworkCore;
using SIMS.DatabaseContext;
using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SimsDbContext _context;

        public UserRepository(SimsDbContext simsDb)
        {
            _context = simsDb;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            // Matches the Task<User?> GetUserByIdAsync(int id) signature
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            // Filters users by the provided role string (e.g., "Student")
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Find the existing entity in the context first
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return false;

            // Apply values from the posted model to the tracked entity
            // This avoids issues with primary key tracking and binding
            _context.Entry(existing).CurrentValues.SetValues(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}