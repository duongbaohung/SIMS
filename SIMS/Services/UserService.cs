using SIMS.DatabaseContext.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<User>> GetAllStudentsAsync()
        {
            return await _userRepo.GetUsersByRoleAsync("Student");
        }

        public async Task<User?> GetStudentByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _userRepo.GetUserByIdAsync(id);
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

        public async Task<bool> UpdateStudentAsync(User student)
        {
            // Apply business rules if necessary (e.g., ensuring role isn't changed)
            if (student.Id <= 0) return false;

            return await _userRepo.UpdateUserAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            if (id <= 0) return false;
            return await _userRepo.DeleteUserAsync(id);
        }
    }
}