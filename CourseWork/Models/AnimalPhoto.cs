using System.Runtime.InteropServices.JavaScript;

namespace CourseWork.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("animal_photos")]
public class AnimalPhoto
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("animal_id")]
    public int AnimalId { get; set; }
    
    [Required]
    [Column("file_path")]
    public string FilePath { get; set; } = string.Empty;
    
    [Required]
    [Column("is_main")]
    public bool IsMain { get; set; }
    
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [ForeignKey("AnimalId")]
    public virtual Animal Animal { get; set; } = null!;
}