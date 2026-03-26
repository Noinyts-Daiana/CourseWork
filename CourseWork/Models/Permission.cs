using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;

[Table("permissions")]
public class Permission
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
}