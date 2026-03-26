using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;

[Table("role_permissions")]
public class RolePermission
{
    [Key]
    public int Id { get; set; }
    
    [Required] [Column("role_id")]
    public int RoleId { get; set; }
    
    [Required] [Column("permission_id")]
    public int PermissionId { get; set; }
    
}