
using CourseWork.Models;
using CourseWork.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction)
    {
        await context.Transaction.AddAsync(transaction);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync()
    {
        return await context.Transaction
            .Include(t => t.Category)
            .Include(t => t.User)
            .OrderByDescending(t => t.TransactionDate) 
            .ToListAsync();  
    }

    public async Task<IEnumerable<TransactionCategories>> GetCategoriesAsync()
    {
        return await context.TransactionCategorie
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}