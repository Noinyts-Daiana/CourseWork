namespace CourseWork.DTOs;
using CourseWork.Models;

public class AnimalDto
{
    public required string Name { get; set; }
    public int SpeciesId { get; set; }
    public int BreedId { get; set; }
    public string? SpeciesName { get; set; } 
    public string? BreedName { get; set; }
    public DateTime Birthday { get; set; }
    public Sex Sex { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public bool IsSterilized { get; set; }
}