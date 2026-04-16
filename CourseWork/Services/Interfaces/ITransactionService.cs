using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Repositories;

namespace CourseWork.Services;

public interface ITransactionService
{
    Task<TransactionDto> CreateTransactionAsync(TransactionDto dto);
    Task<IEnumerable<TransactionDto>> GetJournalAsync();
    Task<IEnumerable<TransactionCategoryDto>> GetCategoriesAsync();
}
