using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CourseWork.Models;

[Table("transaction_categories")]
public class TransactionCategories
{
    [Key]
    public  int Id { get; set; }
    
    [Required]
    [Column("name")]
    public string? Name { get; set; }
}