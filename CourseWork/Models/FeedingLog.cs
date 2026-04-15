using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CourseWork.Models;

[Table("fooding_logs")]
public class FeedingLog
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("animal_id")]
    public int AnimalId { get; set; }
    
    public Animal? Animal { get; set; }
    
    [Required]
    [Column("food_type_id")]
    public int FoodTypeId { get; set; }
    
    public FoodType? FoodType { get; set; }
    
    [Required]
    [Column("fed_at")]
    public DateTime FedAt { get; set; }
    
    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Required]
    [Column("fed_by_id")]
    public int FedById { get; set; }
    
    public User? FedBy { get; set; }
    
}