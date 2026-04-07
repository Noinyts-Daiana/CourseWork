using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class UserMappers
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto()
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive,
        };
    }

    public static User ToEntity(this UserDto dto)
    {
        return new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Password = dto.Password ?? string.Empty,
            RoleId = dto.RoleId,
            CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = dto.UpdatedAt,
            IsActive = dto.IsActive,
        };
    }
    
}
