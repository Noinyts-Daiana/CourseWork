using CourseWork.DTOs;

namespace CourseWork.Services;

public interface ISpecieService
{
    Task<SpeciesDto?> GetSpecieAsync(int id);
    Task<IEnumerable<SpeciesDto>> GetAllSpeciesAsync();
    Task AddSpecieAsync(SpeciesDto specie);
    Task<bool> UpdateSpecieAsync(int id, SpeciesDto specie);
    Task DeleteSpecieAsync(int id);
}