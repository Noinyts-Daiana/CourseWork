using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Repositories;
using CourseWork.Services;

public class TransactionService(ITransactionRepository repository) : ITransactionService
{
    public async Task<TransactionDto> CreateTransactionAsync(TransactionDto dto)
    {
        var transaction = dto.ToEntity();
        await repository.AddAsync(transaction);
        return transaction.ToDto();
    }

    public async Task<IEnumerable<TransactionDto>> GetJournalAsync()
    {
        var transactions = await repository.GetAllWithDetailsAsync();
        return transactions.Select(t => t.ToDto());
    }

    public async Task<IEnumerable<TransactionCategoryDto>> GetCategoriesAsync()
    {
        var categories = await repository.GetCategoriesAsync();
        return categories.Select(c => c.ToDto());
    }
}