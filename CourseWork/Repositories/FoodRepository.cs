using CourseWork.Models; 
using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class FoodRepository(AppDbContext context) : IFoodRepository
{
    public async Task<IEnumerable<FoodType>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = context.FoodType.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(f => 
                EF.Functions.ILike(f.Name, $"%{term}%") || 
                EF.Functions.ILike(f.Brand, $"%{term}%"));
        }

        return await query
            .OrderBy(f => f.Id) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync(string? searchTerm)
    {
        var query = context.FoodType.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(f => 
                f.Name.ToLower().Contains(term) || 
                f.Brand.ToLower().Contains(term));
        }

        return await query.CountAsync();
    }

    public async Task<FoodType?> GetByIdAsync(int id)
    {
        return await context.FoodType.FindAsync(id);
    }

    public async Task AddAsync(FoodType foodType)
    {
        await context.FoodType.AddAsync(foodType);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FoodType foodType)
    {
        context.FoodType.Update(foodType);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var foodType = await GetByIdAsync(id);
        if (foodType != null)
        {
            context.FoodType.Remove(foodType);
            await context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<string>> GetUniqueBrandsAsync(string? searchTerm, int pageNumber, int pageSize)
    {
        var query = context.FoodType
            .Select(f => f.Brand)
            .Distinct();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(b => b != null && b.ToLower().Contains(term));
        }

        return await query
            .OrderBy(b => b) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetUniqueBrandsCountAsync(string? searchTerm)
    {
        var query = context.FoodType
            .Select(f => f.Brand)
            .Distinct();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(b => b != null && b.ToLower().Contains(term));
        }

        return await query.CountAsync();
    }
}