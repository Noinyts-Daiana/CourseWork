using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;

[Table("adopt_animals")]
public class AdoptAnimal
{
    [Key]
    public int Id { get; set; }

    [Required] [Column("animal_id")] 
    public int AnimalId { get; set; }
    
    public Animal Animal { get; set; }

    [Column("owner_id")]
    public int? OwnerId { get; set; }
    
    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime ArrivalDate { get; set; } 
    
    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime? AdoptDate { get; set; }
    
    
}