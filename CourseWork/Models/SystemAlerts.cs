using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CourseWork.Models;


[Table("system_alerts")]
public class SystemAlerts
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("type")]
    public string? Type { get; set; }
    
    [Required]
    [Column("message")]
    public string? Message { get; set; }
    
    [Required]
    [Column("is_done")]
    public bool IsDone { get; set; }
    
}