using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories.Interfaces; 
using CourseWork.Services.Interfaces; 

namespace CourseWork.Services;

public class TransactionService(ITransactionRepository repository) : ITransactionService
{
    public async Task<TransactionDto> CreateTransactionAsync(TransactionDto dto)
    {
        int finalCategoryId;

        if (dto.CategoryId.HasValue && dto.CategoryId > 0)
        {
            finalCategoryId = dto.CategoryId.Value;
        }
        else if (!string.IsNullOrWhiteSpace(dto.CategoryName))
        {
            var existingCategory = await repository.GetCategoryByNameAsync(dto.CategoryName);
            
            if (existingCategory != null)
            {
                finalCategoryId = existingCategory.Id; 
            }
            else
            {
                var newCat = new TransactionCategories { Name = dto.CategoryName };
                var createdCat = await repository.AddCategoryAsync(newCat);
                finalCategoryId = createdCat.Id;
            }
        }
        else
        {
            throw new ArgumentException("Вкажіть існуючу категорію (CategoryId) або введіть нову назву (CategoryName).");
        }

        var transaction = dto.ToEntity();
        transaction.CategoryId = finalCategoryId; 

        await repository.AddAsync(transaction);
        
        var resultDto = transaction.ToDto();
        resultDto.CategoryName = dto.CategoryName; 
        
        return resultDto;
    }

    public async Task<IEnumerable<TransactionDto>> GetJournalAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize)
    {
        var transactions = await repository.GetJournalAsync(searchTerm, fromDate, toDate, pageNumber, pageSize);
        return transactions.Select(t => t.ToDto());
    }

    public async Task<int> GetJournalCountAsync(string? searchTerm, DateTime? fromDate, DateTime? toDate)
    {
        return await repository.GetJournalCountAsync(searchTerm, fromDate, toDate);
    }

    public async Task<IEnumerable<TransactionCategoryDto>> GetCategoriesAsync(string? searchTerm, int pageNumber, int pageSize)
    {
        var categories = await repository.GetCategoriesAsync(searchTerm, pageNumber, pageSize);
        return categories.Select(c => c.ToDto());
    }

    public async Task<int> GetCategoriesCountAsync(string? searchTerm)
    {
        return await repository.GetCategoriesCountAsync(searchTerm);
    }
    public async Task<TransactionDto?> UpdateTransactionAsync(int id, TransactionDto dto)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction == null) return null;

        int finalCategoryId = transaction.CategoryId;
        if (dto.CategoryId.HasValue && dto.CategoryId > 0) {
            finalCategoryId = dto.CategoryId.Value;
        } else if (!string.IsNullOrWhiteSpace(dto.CategoryName)) {
            var existingCategory = await repository.GetCategoryByNameAsync(dto.CategoryName);
            if (existingCategory != null) {
                finalCategoryId = existingCategory.Id; 
            } else {
                var newCat = new TransactionCategories { Name = dto.CategoryName };
                var createdCat = await repository.AddCategoryAsync(newCat);
                finalCategoryId = createdCat.Id;
            }
        }

        transaction.Amount = dto.Amount;
        transaction.IsIncome = dto.IsIncome;
        transaction.Description = dto.Description;
        transaction.CategoryId = finalCategoryId;

        await repository.UpdateAsync(transaction);
        
        return dto;
    }
}