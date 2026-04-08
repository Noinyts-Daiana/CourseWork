using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class SpecieRepository(AppDbContext context): ISpecieRepository
{
    public async Task<Specie?> GetSpecieAsync(int id)
    {
        return await context.Specie.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Specie>> GetAllSpeciesAsync()
    {
        return await context.Specie.ToListAsync();
    }

    public async Task AddSpecieAsync(Specie specie)
    {
        context.Specie.Add(specie);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSpecieAsync(Specie specie)
    {
        context.Specie.Update(specie);
        await context.SaveChangesAsync();
    }

    public async Task DeleteSpecieAsync(int id)
    {
        var specie = await GetSpecieAsync(id);
        context.Specie.Remove(specie);
        
    }
    
}