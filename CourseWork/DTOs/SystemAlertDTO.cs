namespace CourseWork.DTOs;

public class SystemAlertDto
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Message { get; set; }
    public string? Severity { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public bool IsDone { get; set; }
    public bool IsAuto { get; set; } 
}