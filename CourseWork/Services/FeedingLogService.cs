using CourseWork.DTOs;
using CourseWork.Mappers; 
using CourseWork.Models; 
using CourseWork.Repositories.Interfaces;
using CourseWork.Services.Interfaces;

namespace CourseWork.Services;

public class FeedingLogService(
    IFeedingLogRepository logRepository, 
    IFoodRepository foodRepository) : IFeedingLogService
{
    public async Task AddFeedingLogAsync(FeedingLogDto dto)
    {
        var food = await foodRepository.GetByIdAsync(dto.FoodTypeId);
        if (food == null) throw new KeyNotFoundException("Корм не знайдено на складі");

        if (food.StockQuantity < dto.Amount)
            throw new InvalidOperationException($"Недостатньо корму! Залишок: {food.StockQuantity} {food.Unit}");

        var log = dto.ToEntity(); 

        await logRepository.AddAsync(log);

        food.StockQuantity -= dto.Amount;
        await foodRepository.UpdateAsync(food);
    }

    public async Task<IEnumerable<FeedingLogDto>> GetLogsByAnimalAsync(int animalId)
    {
        var logs = await logRepository.GetByAnimalIdAsync(animalId);
        
        return logs.Select(log => log.ToDto());
    }

    public async Task<IEnumerable<FeedingLogDto>> GetRecentLogsAsync(int count = 50)
    {
        var logs = await logRepository.GetRecentLogsAsync(count);
        
        return logs.Select(log => log.ToDto());
    }
}