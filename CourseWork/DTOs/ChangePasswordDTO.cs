using System.ComponentModel.DataAnnotations;

namespace CourseWork.DTOs;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Поточний пароль обов'язковий")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Новий пароль обов'язковий")]
    [MinLength(6, ErrorMessage = "Мінімум 6 символів")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
    
    [Compare("NewPassword", ErrorMessage = "Новий пароль та підтвердження не збігаються!")]
    public string ConfirmPassword { get; set; } = string.Empty;
}