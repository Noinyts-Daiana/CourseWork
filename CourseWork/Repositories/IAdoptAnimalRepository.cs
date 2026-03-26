using CourseWork.Models;

namespace CourseWork.Repositories;

public interface IAdoptAnimalRepository
{
    Task<AdoptAnimal?>  GetByIdAsync(int id);
    Task<IEnumerable<AdoptAnimal>> GetAllAsync();
    Task CreateAdoptAnimal(AdoptAnimal adoptAnimal);
    Task UpdateAdoptAnimal(AdoptAnimal adoptAnimal);
    Task<bool> DeleteAdoptAnimal(int id);
}