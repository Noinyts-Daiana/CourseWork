using Microsoft.EntityFrameworkCore;
using CourseWork.Models;

namespace CourseWork.Repositories;

public class BreedRepository(AppDbContext context) : IBreedRepository
{
    public async Task<IEnumerable<Breed>> GetAllAsync()
    {
        return await context.Breeds.ToListAsync();
    }

    public async Task<Breed?> GetByIdAsync(int id)
    {
        return await context.Breeds.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Breed breed)
    {
        context.Breeds.Add(breed);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Breed breed)
    {
        context.Breeds.Update(breed);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Breed breed)
    {
        context.Breeds.Remove(breed);
        await context.SaveChangesAsync();
    }

}