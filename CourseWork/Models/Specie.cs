using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;


[Table("species")]
public class Specie
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Column("slug")]
    public string Slug { get; set; } = string.Empty;
}