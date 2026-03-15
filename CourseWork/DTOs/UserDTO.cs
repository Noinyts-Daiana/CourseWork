using System.ComponentModel.DataAnnotations;

namespace CourseWork.DTOs;

public class UserCreateDto
{
    [Required]
    [MinLength(3)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty; 

    public int RoleId { get; set; }
}