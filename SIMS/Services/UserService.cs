using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;
namespace SIMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository repository)
        {
            _userRepo = repository;
        }
        public async Task<User?> LoginUserAsync(string username, string password)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return null;

            return user.HashPassword.Equals(password) ? user : null;
        } 
    }
}
