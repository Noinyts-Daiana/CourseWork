namespace CourseWork.DTOs;
using CourseWork.Models;

public class AnimalDto
{
    public int Id { get; set; } 
    public required string Name { get; set; }
    public int? SpeciesId { get; set; }
    public int? BreedId { get; set; }
    public List<int>? CharacteristicIds { get; set; } 
    public string? SpeciesName { get; set; } 
    public string? BreedName { get; set; }
    public List<string>? Characteristics { get; set; } 
    public List<string>? NewCharacteristicNames { get; set; }

    public DateTime? Birthday { get; set; } 
    public Sex Sex { get; set; }   
    public decimal Weight { get; set; }    
    public decimal Height { get; set; }
    public bool IsSterilized { get; set; }
    public string? Description { get; set; }
    public List<AnimalPhotoDto> Photos { get; set; } = new();
}