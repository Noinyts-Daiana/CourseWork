namespace CourseWork.DTOs;

public class FoodTypesDto
{
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Unit { get; set; }
    public decimal StockQuantity { get; set; }
    public decimal MinThreshold { get; set; }
}