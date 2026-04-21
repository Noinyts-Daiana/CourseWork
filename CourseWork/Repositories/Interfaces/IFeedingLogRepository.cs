using CourseWork.DTOs;
using CourseWork.Models;

public interface IFeedingLogRepository
{
    Task AddAsync(FeedingLog log);
    Task<IEnumerable<FeedingLog>> GetByAnimalIdAsync(int animalId);
    Task<FeedingLog?> GetByIdAsync(int id);
    Task<IEnumerable<FeedingLog>> GetRecentLogsAsync(int count = 50);
    Task UpdateAsync(FeedingLog log);
    Task DeleteAsync(FeedingLog log);
}