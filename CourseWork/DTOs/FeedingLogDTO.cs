namespace CourseWork.DTOs;

public class FeedingLogDto
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public int FoodTypeId { get; set; }
    public decimal Amount { get; set; }
    public int FedById { get; set; } 

    public string? FoodName { get; set; }
    public string? FoodBrand { get; set; }
    public string? Unit { get; set; }
    public string? FedByName { get; set; }
    public DateTime FedAt { get; set; }
}