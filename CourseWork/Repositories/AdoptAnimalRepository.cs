using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class AdoptAnimalRepository(AppDbContext context): IAdoptAnimalRepository
{
    public async Task<AdoptAnimal?> GetByIdAsync(int id)
    {
        return await context.AdoptAnimal.FirstOrDefaultAsync(a=>a.Id == id);
    }

    public async Task<IEnumerable<AdoptAnimal>> GetAllAsync()
    {
        return await context.AdoptAnimal
            .AsNoTracking() 
            .ToListAsync();
    }

    public async Task UpdateAdoptAnimal(AdoptAnimal adoptAnimal)
    {
        context.AdoptAnimal.Update(adoptAnimal);
        await context.SaveChangesAsync();
    }

    public async Task CreateAdoptAnimal(AdoptAnimal adoptAnimal)
    {
        context.AdoptAnimal.Add(adoptAnimal);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAdoptAnimal(int id)
    {
        AdoptAnimal? adoptAnimal = context.AdoptAnimal.FirstOrDefault(a => a.Id == id);

        if (adoptAnimal != null)
        {
            context.AdoptAnimal.Remove(adoptAnimal);
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<IEnumerable<AdoptAnimal>> GetAvailableAnimalsAsync()
    {
        return await context.AdoptAnimal
            .Include(a => a.Animal)
            .Where(aa => aa.OwnerId == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdoptAnimal>> GetByUserIdAsync(int ownerId)
    {
        var usersAnimal = await context.AdoptAnimal
            .Include(aa => aa.Animal)
            .Where(aa => aa.OwnerId == ownerId)
            .ToListAsync();
        return usersAnimal;
    }
}