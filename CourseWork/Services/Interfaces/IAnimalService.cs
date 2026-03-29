using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Services;

public interface IAnimalService
{
    Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync();
    Task<AnimalDto?> GetAnimalByIdAsync(int animalId);
    Task DeleteAnimalAsync(int animalId);
    Task<AnimalDto> AddAnimalAsync(AnimalDto animalDto);
    Task UpdateAnimalAsync(AnimalDto animalDto);
}