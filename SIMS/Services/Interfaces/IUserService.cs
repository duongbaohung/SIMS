using SIMS.DatabaseContext.Entities;

namespace SIMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> LoginUserAsync(string username, string password);
        Task<IEnumerable<User>> GetAllStudentsAsync();
        Task<bool> AddStudentAsync(User student);
    }
}
