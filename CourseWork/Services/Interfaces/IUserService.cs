using CourseWork.DTOs;

namespace CourseWork.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers(int pageNumber, int pageSize);
    Task<UserDto?> GetUserById(int userId);
    Task<UserDto?> GetUserByEmail(string email);
    Task<UserDto> AddUser(UserDto userDto);
    Task<UserDto?> UpdateUserAsync(int userId, UserDto userDto);
    Task<bool> UpdatePassword(int userId, ChangePasswordDto dto);
    Task DeleteUser(int userId);
    Task<IEnumerable<UserDto>> GetUserByRole(int roleId);
    
    Task<int> GetTotalUsersCountAsync();
    
}