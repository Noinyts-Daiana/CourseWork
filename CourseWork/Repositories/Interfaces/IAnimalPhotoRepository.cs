using CourseWork.Models;
using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public interface IAnimalPhotoRepository
{
    Task<IEnumerable<AnimalPhoto>> GetByAnimalIdAsync(int animalId);
    Task<AnimalPhoto?> GetByIdAsync(int id);
    Task AddAsync(AnimalPhoto photo);
    Task DeleteAsync(AnimalPhoto photo);
    Task ClearMainPhotoFlagAsync(int animalId); 
    Task SaveChangesAsync();
}
