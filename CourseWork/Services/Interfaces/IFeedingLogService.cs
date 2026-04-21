using CourseWork.DTOs;
using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using CourseWork.Services.Interfaces;


public interface IFeedingLogService
{
    Task AddFeedingLogAsync(FeedingLogDto dto);
    Task<IEnumerable<FeedingLogDto>> GetLogsByAnimalAsync(int animalId);
    Task<IEnumerable<FeedingLogDto>> GetRecentLogsAsync(int count = 50);
    Task<FeedingLogDto> UpdateFeedingLogAsync(int id, FeedingLogDto dto);
    Task<bool> DeleteFeedingLogAsync(int id);
}

   