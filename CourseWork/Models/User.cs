using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models; 

[Table("users")]
public class User 
{
    [Key] 
    public int Id { get; set; }
    
    [Required]
    [Column("full_name")] 
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [Column("email")]
    public string Email { get; set; } = string.Empty; 
    
    [Column("role_id")]
    public int RoleId { get; set; }
    
    [Required]
    [Column("password")]
    public string Password { get; set; } = string.Empty;
    
    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    
    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime? UpdatedAt { get; set; } 
    
    [ForeignKey("RoleId")] 
    public Role Role { get; set; } = null!;
}