using CourseWork.DTOs;

namespace CourseWork.Services;

public interface ISystemAlertService
{
    Task<IEnumerable<SystemAlertDto>> GetAllAsync(bool? isDone, int pageNumber, int pageSize);
    Task<SystemAlertDto?> GetByIdAsync(int id);
    Task<SystemAlertDto> CreateAsync(SystemAlertDto dto);
    Task MarkDoneAsync(int id);
    Task DeleteAsync(int id);
    Task<int> GetCountAsync(bool? isDone);
    Task GenerateAutomaticAlertsAsync();
}