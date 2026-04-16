using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class TransactionMappers
{
    public static TransactionDto ToDto(this Transaction t)
    {
        return new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            CategoryId = t.CategoryId,
            Description = t.Description,
            IsIncome = t.IsIncome,
            TransactionDate = t.TransactionDate,
            UserId = t.UserId,
            CategoryName = t.Category?.Name,
            InitiatorName = t.User?.FullName 
        };
    }

    public static Transaction ToEntity(this TransactionDto dto)
    {
        return new Transaction
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            IsIncome = dto.IsIncome,
            TransactionDate = dto.TransactionDate != default ? dto.TransactionDate : DateTime.UtcNow,
            UserId = dto.UserId
        };
    }

    public static TransactionCategoryDto ToDto(this TransactionCategories category)
    {
        return new TransactionCategoryDto
        {
            Id = category.Id,
            Name = category.Name ?? "Без назви"
        };
    }
}