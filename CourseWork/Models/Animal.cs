using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.Models;

public enum Sex
{
    Male = 1,
    Female = 2
}

[Table("animals")]
public class Animal
{
    [Key]
    public int Id { get; set; }

    [Required] [Column("name")] 
    public required string Name { get; set; }

    [Required]
    [Column("species_id")]
    public int SpeciesId { get; set; }
    
    [Required]
    [Column("breed_id")]
    public int BreedId { get; set; }
    
    [Column("birth_date", TypeName = "date")]
    public DateTime? Birthday { get; set; }
    
    [Required]
    [Column("sex")]
    public Sex Sex  { get; set; }
    
    [Required]
    [Column("weight")]
    public decimal Weight { get; set; }
    
    [Required]
    [Column("height")]
    public decimal Height { get; set; }
    
    [Required]
    [Column("is_sterilized")]
    public bool IsSterilized { get; set; } = false;
    
    [Required]
    [Column("description")]
    public string? Description { get; set; }
    
    
    [ForeignKey("SpeciesId")]
    public Specie? Specie { get; set; }

    [ForeignKey("BreedId")]
    public Breed? Breed { get; set; }
    
    public ICollection<AnimalCharacteristic> AnimalCharacteristics { get; set; } = new List<AnimalCharacteristic>();
    public virtual ICollection<AnimalPhoto> Photos { get; set; } = new List<AnimalPhoto>();
  
}