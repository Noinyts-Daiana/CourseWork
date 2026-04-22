using Microsoft.EntityFrameworkCore;
using CourseWork.Models;

namespace CourseWork.Repositories;

public class BreedRepository(AppDbContext context) : IBreedRepository
{
    public async Task<IEnumerable<Breed>> GetAllAsync()
    {
        return await context.Breed
            .Include(b => b.Specie) 
            .ToListAsync();
    }

    public async Task<Breed?> GetByIdAsync(int id)
    {
        return await context.Breed.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Breed breed)
    {
        context.Breed.Add(breed);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Breed breed)
    {
        context.Breed.Update(breed);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Breed breed)
    {
        context.Breed.Remove(breed);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Breed>> GetBreedsByNameAsync(string name)
    {
        return await context.Breed
        .Where(b => b.Name.Contains(name))
        .ToListAsync();
    }
    
    public async Task<IEnumerable<string>> GetUniqueBreedNamesAsync(string? searchTerm, int pageNumber, int pageSize)
    {
        var query = context.Breed
            .Select(b => b.Name)
            .Distinct();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(name => name != null && name.ToLower().Contains(term));
        }

        return await query
            .OrderBy(name => name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetUniqueBreedNamesCountAsync(string? searchTerm)
    {
        var query = context.Breed
            .Select(b => b.Name)
            .Distinct();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(name => name != null && name.ToLower().Contains(term));
        }

        return await query.CountAsync();
    }
    
    public async Task<IEnumerable<Breed>> GetBreedsBySpeciesIdAsync(int speciesId)
    {
        return await context.Breed
            .Where(b => b.SpecieId == speciesId) 
            .OrderBy(b => b.Name)
            .ToListAsync();
    }
}