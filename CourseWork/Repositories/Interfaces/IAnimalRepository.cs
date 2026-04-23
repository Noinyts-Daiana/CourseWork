using CourseWork.Models;

namespace CourseWork.Repositories;

public interface IAnimalRepository
{
    Task<IEnumerable<Animal>> GetAnimalsAsync(int pageNumber, int pageSize, string? searchTerm, List<int>? charIds,
        int? speciesId, int? breedId, int? sex);
    Task<Animal?> GetAnimalByIdAsync(int id);
    Task<Animal> AddAnimalAsync(Animal animal);
    Task UpdateAnimalAsync(Animal animal); 
    Task DeleteAnimalAsync(int id);
    Task<IEnumerable<Animal>> GetAnimalsByNameAsync(string animalName);
    Task<IEnumerable<Animal>> GetAnimalsByBreedAsync(int breedId);
    Task<IEnumerable<Animal>> GetAnimalsBySpeciesAsync(int speciesId);
    Task<IEnumerable<Animal>> GetAnimalsByGenderAsync(Sex sex);
    Task<int> GetAnimalsCountAsync(string? searchTerm, List<int>? charIds, int? speciesId, int? breedId, int? sex);
}