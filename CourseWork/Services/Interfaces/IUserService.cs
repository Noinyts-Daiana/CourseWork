using CourseWork.DTOs;

namespace CourseWork.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers(int pageNumber, int pageSize, string? searchTerm = null, int? roleId = null,  bool? isActive = null);
    Task<int> GetTotalUsersCountAsync(string? searchTerm = null, int? roleId = null);
    Task<bool> ToggleUserStatusAsync(int id);
    Task<UserDto?> GetUserById(int userId);
    Task<UserDto?> GetUserByEmail(string email);
    Task<UserDto> AddUser(UserDto userDto);
    Task<UserDto?> UpdateUserAsync(int userId, UserDto userDto);
    Task<bool> UpdatePassword(int userId, ChangePasswordDto dto);
    Task DeleteUser(int userId);
    Task<IEnumerable<UserDto>> GetUserByRole(int roleId);
    
}