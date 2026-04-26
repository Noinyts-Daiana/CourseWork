using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IFoodRepository
{
    Task<IEnumerable<FoodType>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, bool? isLowStock = null);
    Task<int> GetCountAsync(string? searchTerm, bool? isLowStock = null);
    Task<FoodType?> GetByIdAsync(int id);
    Task AddAsync(FoodType foodType);
    Task UpdateAsync(FoodType foodType);
    Task DeleteAsync(int id);
    Task<IEnumerable<string>> GetUniqueBrandsAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<int> GetUniqueBrandsCountAsync(string? searchTerm);
}