using System.ComponentModel.DataAnnotations;

namespace CourseWork.DTOs;

public class UserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Неправильний формат")]
    public string Email { get; set; } = string.Empty;

    [MinLength(6, ErrorMessage = "Мінімум 6 символів")]
    public string? Password { get; set; }

    public int RoleId { get; set; } = 1;
    public string? RoleName { get; set; } 

    public bool IsActive { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}