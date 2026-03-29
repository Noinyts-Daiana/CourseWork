using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CourseWork.Models;

[Table("food_types")]
public class FoodTypes
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("name")]
    public string? Name { get; set; }
    
    [Required]
    [Column("brand")]
    public string? Brand { get; set; }
    
    [Required]
    [Column("unit")]
    public string? Unit { get; set; }
    
    [Required]
    [Column("stock_quantity")]
    public decimal StockQuantity { get; set; }
    
    [Required]
    [Column("min_threshold")]
    public decimal MinThreshold { get; set; }
}