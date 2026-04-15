using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IFoodRepository
{
    Task<IEnumerable<FoodType>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<int> GetCountAsync(string? searchTerm);
    Task<FoodType?> GetByIdAsync(int id);
    Task AddAsync(FoodType foodType);
    Task UpdateAsync(FoodType foodType);
    Task DeleteAsync(int id);
}