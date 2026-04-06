using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int userId);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<User?> DeleteUserAsync(int userId);
    Task<IEnumerable<User>> GetUserByRole(int roleId);
    
}