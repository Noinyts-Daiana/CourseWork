using CourseWork.Models;

namespace CourseWork.Repositories;

public interface ISystemAlertRepository
{
    Task<IEnumerable<SystemAlerts>> GetAllAsync(bool? isDone, int pageNumber, int pageSize);
    Task<SystemAlerts?> GetByIdAsync(int id);
    Task<SystemAlerts> AddAsync(SystemAlerts alert);
    Task UpdateAsync(SystemAlerts alert);
    Task DeleteAsync(int id);
    Task<int> GetCountAsync(bool? isDone);
    Task<bool> CheckIfExistsAsync(string message, DateTime date);
}