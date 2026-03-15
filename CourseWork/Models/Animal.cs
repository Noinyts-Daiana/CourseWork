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
    
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Column("species_id")]
    public int SpeciesId { get; set; }
    
    [Required]
    [Column("breed_id")]
    public int BreedId { get; set; }
    
    [Required]
    [Column("birth_date")]
    public DateTime Birthday { get; set; }
    
    [Required]
    [Column("sex")]
    public Sex Sex  { get; set; }
  
    [Column("owner_id")]
    public int? OwnerId { get; set; }
    
    [Required]
    [Column("weight")]
    public double Weight { get; set; }
    
    [Required]
    [Column("height")]
    public double Height { get; set; }
    
    [Required]
    [Column("is_sterilized")]
    public bool isSterilized { get; set; } = false;
    
    [Required]
    [Column("is_adopted")]
    public bool isAdopted { get; set; } = false;
    
    [ForeignKey("SpeciesId")]
    public Specie? Specie { get; set; }

    [ForeignKey("BreedId")]
    public Breed? Breed { get; set; }
}