using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces; 

public interface ITransactionRepository
{ 
    Task AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetJournalAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize);
    Task<int> GetJournalCountAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate);
    Task<IEnumerable<TransactionCategories>> GetCategoriesAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<int> GetCategoriesCountAsync(string? searchTerm);
    Task<TransactionCategories?> GetCategoryByNameAsync(string name);
    Task<TransactionCategories> AddCategoryAsync(TransactionCategories category);
    Task UpdateAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<decimal> GetTotalBalanceAsync();
}