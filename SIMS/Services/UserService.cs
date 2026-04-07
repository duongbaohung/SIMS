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

            // Check if password matches AND the user account is active (Status == 1)
            if (user.HashPassword.Equals(password) && user.Status == 1)
            {
                return user;
            }

            return null;
        }
        // Add these below your LoginUserAsync method:

        public async Task<IEnumerable<User>> GetAllStudentsAsync()
        {
            return await _userRepo.GetUsersByRoleAsync("Student");
        }

        public async Task<bool> AddStudentAsync(User student)
        {
            student.Role = "Student";
            student.Status = 1;
            student.CreatedAt = DateTime.Now;

            // Set a default password if one wasn't provided
            if (string.IsNullOrEmpty(student.HashPassword))
            {
                student.HashPassword = "12345678"; // Default password
            }

            return await _userRepo.AddUserAsync(student);
        }
    }
}