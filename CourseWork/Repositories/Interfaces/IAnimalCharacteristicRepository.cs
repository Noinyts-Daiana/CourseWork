using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IAnimalCharacteristicRepository
{
    Task AddAsync(AnimalCharacteristic animalCharacteristic);
    Task RemoveAsync(AnimalCharacteristic animalCharacteristic);
    Task<IEnumerable<Characteristic>> GetCharacteristicsByAnimalIdAsync(int animalId);
    Task<IEnumerable<Animal>> GetAnimalsByCharacteristicIdAsync(int characteristicId);
    Task RemoveAllForAnimalAsync(int animalId);
}