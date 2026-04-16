using CourseWork.DTOs;
using CourseWork.Models; 

namespace CourseWork.Repositories;

public interface IBreedRepository
{
    Task<IEnumerable<Breed>> GetAllAsync();
    Task<Breed?> GetByIdAsync(int id);
    Task AddAsync(Breed breed);
    Task UpdateAsync(Breed breed);
    Task DeleteAsync(Breed breed);
    Task<IEnumerable<Breed>> GetBreedsByNameAsync(string name);
    Task<IEnumerable<string>> GetUniqueBreedNamesAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<int> GetUniqueBreedNamesCountAsync(string? searchTerm);
}