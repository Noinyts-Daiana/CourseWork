using CourseWork.DTOs;
using CourseWork.Models;

public interface IFeedingLogRepository
{
    Task AddAsync(FeedingLog log);
    
    Task<IEnumerable<FeedingLog>> GetByAnimalIdAsync(int animalId);
    
    Task<IEnumerable<FeedingLog>> GetRecentLogsAsync(int count = 50);
}