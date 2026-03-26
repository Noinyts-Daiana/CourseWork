namespace CourseWork.DTOs;

public class AdoptAnimalDto
{
    public int Id { get; set; }
    
    public int AnimalId { get; set; }
    public string? AnimalName { get; set; } 
    
    public int? OwnerId { get; set; }
    public string? OwnerName { get; set; } 
    
    public DateTime ArrivalDate { get; set; }
    public DateTime? AdoptDate { get; set; }
}