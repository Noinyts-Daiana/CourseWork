using CourseWork.DTOs;

namespace CourseWork.Services.Interfaces; 

public interface ITransactionService
{
    Task<TransactionDto> CreateTransactionAsync(TransactionDto dto);
    Task<IEnumerable<TransactionDto>> GetJournalAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize);
    Task<int> GetJournalCountAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate);
    Task<IEnumerable<TransactionCategoryDto>> GetCategoriesAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<int> GetCategoriesCountAsync(string? searchTerm); 
    Task<TransactionDto?> UpdateTransactionAsync(int id, TransactionDto dto);
    Task DeleteTransactionAsync(int id);
}