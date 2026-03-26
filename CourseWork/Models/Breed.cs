using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.Models;

[Table("breeds")]
public class Breed
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("specie_id")]
    public int SpecieId { get; set; }
    
    [Required]
    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [ForeignKey("SpecieId")] 
    public Specie Specie { get; set; } = null!; 
    
    
}