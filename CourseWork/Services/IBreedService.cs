namespace CourseWork.Services;
using CourseWork.DTOs;

public interface IBreedService
{
    Task<int> CreateBreedAsync(BreedsDto breedDto); 
    Task<IEnumerable<BreedsDto>> GetAllBreedsAsync();
    Task<BreedsDto?> GetBreedByIdAsync(int id);
    
    Task<bool> UpdateBreedAsync(int id, BreedsDto breedDto);
    
}
