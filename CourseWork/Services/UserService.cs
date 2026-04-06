using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories.Interfaces;

namespace CourseWork.Services;

public class UserService(IUserRepository userRepository): IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await userRepository.GetUsersAsync();

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
        
        var user = new UserDto()
        {
            FullName = userDto.FullName,
            Email = userDto.Email,
            RoleId = userDto.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
        };
        
        await userRepository.AddUserAsync(user.ToEntity());
        return user;
    }


    public async Task<bool> UpdatePassword(int userId, string newPassword)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return false;
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
    
        user.UpdatedAt = DateTime.UtcNow;
        await userRepository.UpdateUserAsync(user);
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

        await userRepository.UpdateUserAsync(existingUser);

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
}