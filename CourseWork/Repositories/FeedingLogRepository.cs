using CourseWork.Models; // Твій шлях до моделей
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class FeedingLogRepository(AppDbContext context) : IFeedingLogRepository
{
    public async Task AddAsync(FeedingLog log)
    {
        await context.FeedingLog.AddAsync(log);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<FeedingLog>> GetByAnimalIdAsync(int animalId)
    {
        return await context.FeedingLog
            .Include(f => f.FoodType) 
            .Include(f => f.FedBy)    
            .Where(f => f.AnimalId == animalId)
            .OrderByDescending(f => f.FedAt) 
            .ToListAsync();
    }

    public async Task<IEnumerable<FeedingLog>> GetRecentLogsAsync(int count = 50)
    {
        return await context.FeedingLog
            .Include(f => f.Animal)   
            .Include(f => f.FoodType) 
            .Include(f => f.FedBy)    
            .OrderByDescending(f => f.FedAt)
            .Take(count)            
            .ToListAsync();
    }
}