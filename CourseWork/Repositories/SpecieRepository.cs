using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class SpecieRepository(AppDbContext context): ISpecieRepository
{
    public async Task<Specie?> GetSpecieAsync(int id)
    {
        return await context.Species.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Specie>> GetAllSpeciesAsync()
    {
        return await context.Species.ToListAsync();
    }

    public async Task AddSpecieAsync(Specie specie)
    {
        context.Species.Add(specie);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSpecieAsync(Specie specie)
    {
        context.Species.Update(specie);
        await context.SaveChangesAsync();
    }

    public async Task DeleteSpecieAsync(int id)
    {
        var specie = await GetSpecieAsync(id);
        context.Species.Remove(specie);
        
    }
    
}