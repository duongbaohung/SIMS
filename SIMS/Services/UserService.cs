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

        // Inject the user repository via constructor
        public UserService(IUserRepository repository)
        {
            _userRepo = repository;
        }

        /// <summary>
        /// Authenticates a user. Used for the Login process.
        /// </summary>
        public async Task<User?> LoginUserAsync(string username, string password)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return null;

            // Simple string comparison for password (In production, use BCrypt/Hashing)
            if (user.HashPassword.Equals(password) && user.Status == 1)
            {
                return user;
            }

            return null;
        }

        /// <summary>
        /// Fetches a user by username. Used for duplicate checking in the Controller.
        /// </summary>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            return await _userRepo.GetUserByUsername(username);
        }

        /// <summary>
        /// Fetches a user by email. Used for duplicate checking in the Controller.
        /// </summary>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _userRepo.GetUserByEmailAsync(email);
        }

        /// <summary>
        /// Retrieves all users with the "Student" role.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllStudentsAsync()
        {
            return await _userRepo.GetUsersByRoleAsync("Student");
        }

        /// <summary>
        /// Retrieves a specific student by ID.
        /// </summary>
        public async Task<User?> GetStudentByIdAsync(int id)
        {
            if (id <= 0) return null;
            return await _userRepo.GetUserByIdAsync(id);
        }

        /// <summary>
        /// Registers a new student and applies mandatory business rules.
        /// </summary>
        public async Task<bool> AddStudentAsync(User student)
        {
            // Business Rule: New records must have these defaults
            student.Role = "Student";
            student.Status = 1; // 1 = Active
            student.CreatedAt = DateTime.Now;

            // Business Rule: Default password if none provided
            if (string.IsNullOrEmpty(student.HashPassword))
            {
                student.HashPassword = "12345678";
            }

            return await _userRepo.AddUserAsync(student);
        }

        /// <summary>
        /// Updates an existing student's details.
        /// </summary>
        public async Task<bool> UpdateStudentAsync(User student)
        {
            if (student.Id <= 0) return false;
            return await _userRepo.UpdateUserAsync(student);
        }

        /// <summary>
        /// Removes a student from the database.
        /// </summary>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            if (id <= 0) return false;
            return await _userRepo.DeleteUserAsync(id);
        }
    }
}