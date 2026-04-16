using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAllWithDetailsAsync();
    Task<IEnumerable<TransactionCategories>> GetCategoriesAsync();
}