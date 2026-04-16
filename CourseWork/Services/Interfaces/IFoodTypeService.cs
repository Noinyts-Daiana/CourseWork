using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Services.Interfaces;

public interface IFoodTypeService
{
    Task<IEnumerable<FoodTypeDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<int> GetCountAsync(string? searchTerm);
    Task<FoodTypeDto?> GetByIdAsync(int id);
    Task AddAsync(FoodTypeDto foodTypeDto);
    Task UpdateAsync(int id, FoodTypeDto foodTypeDto);
    Task DeleteAsync(int id);
    Task AdjustStockAsync(int id, decimal amountChange);
    Task<IEnumerable<string>> GetBrandsAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<int> GetBrandsCountAsync(string? searchTerm);
    
}