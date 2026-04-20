using CourseWork.DTOs;
using CourseWork.Models; 

namespace CourseWork.Mappers;

public static class FeedingLogMappers
{
    public static FeedingLogDto ToDto(this FeedingLog log)
    {
        return new FeedingLogDto
        {
            Id = log.Id,
            AnimalId = log.AnimalId,
            FoodTypeId = log.FoodTypeId,
            Amount = log.Amount,
            FedById = log.FedById,
            FedAt = log.FedAt,
            
            FoodName = log.FoodType?.Name,
            FoodBrand = log.FoodType?.Brand,
            Unit = log.FoodType?.Unit,
            FedByName = log.FedBy?.FullName 
        };
    }

    public static FeedingLog ToEntity(this FeedingLogDto dto)
    {
        return new FeedingLog
        {
            AnimalId = dto.AnimalId,
            FoodTypeId = dto.FoodTypeId,
            Amount = dto.Amount,
            FedById = dto.FedById,
            
            FedAt = dto.FedAt != default ? dto.FedAt.ToUniversalTime() : DateTime.UtcNow
        };
    }
}