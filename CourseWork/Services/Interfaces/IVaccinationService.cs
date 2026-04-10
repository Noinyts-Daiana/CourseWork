using CourseWork.DTOs;

namespace CourseWork.Repositories.Interfaces;

public interface IVaccinationService
{
    Task<IEnumerable<VaccinationDto>> GetAllVaccinationsAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<VaccinationDto?> GetVaccinationAsync(int id);
    Task AddVaccinationAsync(VaccinationDto vaccination);
    Task UpdateVaccinationAsync(int id, VaccinationDto vaccination);
    Task DeleteVaccinationAsync(int id);
    Task<int> GetVaccinationsCountAsync();
}