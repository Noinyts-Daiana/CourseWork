using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CourseWork.Models;

[Table("transactions")]
public class Transaction
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Required]
    [Column("category_id")]
    public int CategoryId { get; set; }
    
    public TransactionCategories? Category { get; set; }
    
    [Required]
    [Column("description")]
    public string? Description { get; set; }
    
    [Required]
    [Column("is_income")]
    public bool IsIncome { get; set; }
    
    [Required]
    [Column("transaction_date")]
    public DateTime TransactionDate { get; set; }
    
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }
    
    public User? User { get; set; }
}