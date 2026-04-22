namespace CourseWork.Services;
using CourseWork.DTOs;

public interface IBreedService
{
    Task<int> CreateBreedAsync(BreedsDto breedDto); 
    Task<IEnumerable<BreedsDto>> GetAllBreedsAsync();
    Task<BreedsDto?> GetBreedByIdAsync(int id);
    Task<bool> UpdateBreedAsync(int id, BreedsDto breedDto);
    Task<IEnumerable<BreedsDto>> GetBreedsByNameAsync(string name);
    Task<IEnumerable<string>> GetUniqueBreedNamesAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<int> GetUniqueBreedNamesCountAsync(string? searchTerm);
    Task<IEnumerable<BreedsDto>> GetBreedsBySpeciesIdAsync(int speciesId);
}
