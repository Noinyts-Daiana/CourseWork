using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;

public enum AdoptionStatus
{
    Adopted = 0,    
    Returned = 1,  
    Fostered = 2    
}

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
    
    [Column("arrival_at", TypeName = "timestamp with time zone")]
    public DateTime ArrivalDate { get; set; } 
    
    [Column("adopt_at", TypeName = "timestamp with time zone")]
    public DateTime? AdoptDate { get; set; }
    
    [Column("status")]
    public AdoptionStatus Status { get; set; }
}