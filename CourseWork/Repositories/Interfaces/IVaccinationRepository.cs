using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IVaccinationRepository
{
    Task<IEnumerable<Vaccination>> GetAllVaccinationsAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<Vaccination?> GetVaccinationAsync(int id);
    Task AddVaccinationAsync(Vaccination vaccination);
    Task UpdateVaccinationAsync(int id, Vaccination vaccination);
    Task DeleteVaccinationAsync(int id);
    Task<int> GetVaccinationsCountAsync();
}