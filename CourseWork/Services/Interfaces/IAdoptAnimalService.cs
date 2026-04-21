using CourseWork.DTOs;

namespace CourseWork.Services;

public interface IAdoptAnimalService
{
    
    Task<AdoptAnimalDto> AdoptAnimalAsync(int animalId, int ownerId, DateTime? date = null);
    
    Task<IEnumerable<AdoptAnimalDto>> GetAvailableAnimalsAsync();
    
    Task<IEnumerable<AdoptAnimalDto>> GetUserAdoptionsAsync(int ownerId);
    Task<AdoptAnimalDto> RegisterArrivalAsync(int animalId, int? previousOwnerId = null);
    Task<AdoptAnimalDto> AdoptAnimalAsync(int animalId, int ownerId);
}
