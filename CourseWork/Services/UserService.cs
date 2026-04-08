using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories.Interfaces;

namespace CourseWork.Services;

public class UserService(IUserRepository userRepository): IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsers(int pageNumber, int pageSize, string? searchTerm)
    {
        var users = await userRepository.GetUsersAsync( pageNumber, pageSize, searchTerm);

        var usersDto = users.Select(u => u.ToDto());
        
        return usersDto;
    }

    public async Task<UserDto?> GetUserById(int userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        var userDto = user?.ToDto();
        return userDto;
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await userRepository.GetByEmailAsync(email);
        var userDto = user?.ToDto();
        return userDto;
    }

    public async Task<UserDto> AddUser(UserDto userDto)
    {
        var existingUser = await userRepository.GetByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Користувач з таким Email вже існує.");
        }
    
        var userEntity = userDto.ToEntity();
        userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        userEntity.IsActive = true;
        userEntity.CreatedAt = DateTime.UtcNow;

        await userRepository.AddUserAsync(userEntity);
    
        return userEntity.ToDto();
    }


    public async Task<bool> UpdatePassword(int userId, ChangePasswordDto dto)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password);
        if (!isCurrentPasswordValid) 
        {
            throw new InvalidOperationException("Поточний пароль введено неправильно.");
        }

        bool isSameAsOld = BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.Password);
        if (isSameAsOld)
        {
            throw new InvalidOperationException("Новий пароль не може бути таким самим, як поточний.");
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
    
        user.UpdatedAt = DateTime.UtcNow;
        await userRepository.UpdateUserAsync(userId, user);
        return true;
    }
    
    public async Task<UserDto?> UpdateUserAsync(int userId, UserDto userDto)
    {
        var existingUser = await userRepository.GetUserByIdAsync(userId);
    
        if (existingUser == null)
        {
            return null;
        }

        existingUser.FullName = userDto.FullName;
        existingUser.Email = userDto.Email;
        existingUser.RoleId = userDto.RoleId;
        existingUser.IsActive = userDto.IsActive;
    
        existingUser.UpdatedAt = DateTime.UtcNow;

        await userRepository.UpdateUserAsync(userId, existingUser);

        return existingUser.ToDto();
    }
    
    public async Task DeleteUser(int userId)
    {
        await userRepository.DeleteUserAsync(userId);   
    }

    public async Task<IEnumerable<UserDto>> GetUserByRole(int roleId)
    {
        var users = await userRepository.GetUserByRole(roleId);
        var usersDto = users.Select(u => u.ToDto());
        return usersDto;
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        var users = await userRepository.GetTotalUsersCountAsync();
        return users;
    }
}