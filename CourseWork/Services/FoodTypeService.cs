using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Repositories.Interfaces;
using CourseWork.Services.Interfaces;

namespace CourseWork.Services;

public class FoodTypeService(IFoodRepository foodRepository): IFoodTypeService
{
    public async Task<IEnumerable<FoodTypeDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var foodTypes = await foodRepository.GetAllAsync(pageNumber, pageSize, searchTerm);
        return foodTypes.Select(f => f.ToDto());
    }

    public async Task<int> GetCountAsync(string? searchTerm)
    {
        return await foodRepository.GetCountAsync(searchTerm);
    }

    public async Task<FoodTypeDto?> GetByIdAsync(int id)
    {
        return (await foodRepository.GetByIdAsync(id))?.ToDto();
    }

    public async Task AddAsync(FoodTypeDto foodTypeDto)
    { 
        await foodRepository.AddAsync(foodTypeDto.ToEntity());
    }
    public async Task UpdateAsync(int id, FoodTypeDto foodTypeDto)
    {
        if (id != foodTypeDto.Id)
        {
            throw new ArgumentException("ID у запиті не збігається з ID у тілі об'єкта");
        }

        var existingFood = await foodRepository.GetByIdAsync(id);
        if (existingFood == null)
        {
            throw new KeyNotFoundException($"Тип корму з ID {id} не знайдено");
        }

        existingFood.Name = foodTypeDto.Name;
        existingFood.Brand = foodTypeDto.Brand;
        existingFood.Unit = foodTypeDto.Unit;
        existingFood.StockQuantity = foodTypeDto.StockQuantity;
        existingFood.MinThreshold = foodTypeDto.MinThreshold;

        await foodRepository.UpdateAsync(existingFood);
    }
    
    public async Task DeleteAsync(int id)
    {
        var foodExists = await foodRepository.GetByIdAsync(id);
        if (foodExists == null)
        {
            throw new KeyNotFoundException($"Неможливо видалити: запис з ID {id} не існує");
        }

        await foodRepository.DeleteAsync(id);
    }
    

    public async Task AdjustStockAsync(int id, decimal amountChange)
    {
        var food = await foodRepository.GetByIdAsync(id);
        if (food == null) throw new KeyNotFoundException($"Корм з ID {id} не знайдено");

        if (food.StockQuantity + amountChange < 0)
            throw new InvalidOperationException("Неможливо списати більше, ніж є на складі.");

        food.StockQuantity += amountChange;
        await foodRepository.UpdateAsync(food);
    }
}