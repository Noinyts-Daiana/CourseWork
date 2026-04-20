namespace CourseWork.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    
    public int? CategoryId { get; set; } 
    
    public string? Description { get; set; }
    public bool IsIncome { get; set; }
    public DateTime TransactionDate { get; set; }
    public int UserId { get; set; }

    public string? CategoryName { get; set; }
    public string? InitiatorName { get; set; } 
}