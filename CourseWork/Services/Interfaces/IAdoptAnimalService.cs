using CourseWork.DTOs;

namespace CourseWork.Services;

public interface IAdoptAnimalService
{
    Task<AdoptAnimalDto> RegisterArrivalAsync(int animalId, DateTime? date = null);
    
    Task<AdoptAnimalDto> AdoptAnimalAsync(int animalId, int ownerId, DateTime? date = null);
    
    Task<IEnumerable<AdoptAnimalDto>> GetAvailableAnimalsAsync();
    
    Task<IEnumerable<AdoptAnimalDto>> GetUserAdoptionsAsync(int ownerId);
}