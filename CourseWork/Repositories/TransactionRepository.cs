using CourseWork.Models;
using CourseWork.Repositories.Interfaces; 
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction)
    {
        await context.Transaction.AddAsync(transaction);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetJournalAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
    {
        var query = context.Transaction
            .Include(t => t.Category)
            .Include(t => t.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(t => 
                (t.Description != null && t.Description.ToLower().Contains(term)) ||
                (t.Category != null && t.Category.Name != null && t.Category.Name.ToLower().Contains(term) ||
                 (t.User.FullName.Contains(searchTerm) || t.User.Email.Contains(searchTerm)))
            );
        }

        if (fromDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            var endDate = toDate.Value.Date.AddDays(1); 
            query = query.Where(t => t.TransactionDate < endDate);
        }

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetJournalCountAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate)
    {
        var query = context.Transaction.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(t => 
                (t.Description != null && t.Description.ToLower().Contains(term)) ||
                (t.Category != null && t.Category.Name != null && t.Category.Name.ToLower().Contains(term) ||
                    (t.User != null && t.User.FullName != null && t.User.FullName.ToLower().Contains(term)))
            );
        }

        if (fromDate.HasValue) 
            query = query.Where(t => t.TransactionDate >= fromDate.Value);
            
        if (toDate.HasValue) 
        {
            var endDate = toDate.Value.Date.AddDays(1);
            query = query.Where(t => t.TransactionDate < endDate);
        }

        return await query.CountAsync();
    }

    public async Task<IEnumerable<TransactionCategories>> GetCategoriesAsync(string? searchTerm, int pageNumber, int pageSize)
    {
        var query = context.TransactionCategorie.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(c => c.Name != null && c.Name.ToLower().Contains(term));
        }

        return await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetCategoriesCountAsync(string? searchTerm)
    {
        var query = context.TransactionCategorie.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(c => c.Name != null && c.Name.ToLower().Contains(term));
        }
        
        return await query.CountAsync();
    }

    public async Task<TransactionCategories?> GetCategoryByNameAsync(string name)
    {
        return await context.TransactionCategorie
            .FirstOrDefaultAsync(c => c.Name != null && c.Name.ToLower() == name.ToLower());
    }

    public async Task<TransactionCategories> AddCategoryAsync(TransactionCategories category)
    {
        await context.TransactionCategorie.AddAsync(category);
        await context.SaveChangesAsync();
        return category; 
    }
    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await context.Transaction.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        context.Transaction.Update(transaction);
        await context.SaveChangesAsync();
    }
}