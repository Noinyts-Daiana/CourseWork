using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Services;

public interface IAnimalService
{
    Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync(int pageNumber, int pageSize, string? searchTerm,
        List<int>? charIds, int? speciesId, int? breedId, int? sex);
    Task<AnimalDto?> GetAnimalByIdAsync(int animalId);
    Task DeleteAnimalAsync(int animalId);
    Task<AnimalDto> AddAnimalAsync(AnimalDto animalDto);
    Task UpdateAnimalAsync(AnimalDto animalDto);
    Task<int> GetAnimalsCountAsync(string? searchTerm, List<int>? charIds, int? speciesId, int? breedId, int? sex);
}