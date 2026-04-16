namespace CourseWork.DTOs;

public class FoodTypeDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Unit { get; set; }
    public decimal StockQuantity { get; set; }
    public decimal MinThreshold { get; set; }
    public decimal Amount { get; set; }

    public bool IsLowStock { get; set; }
}