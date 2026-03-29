using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class AdoptAnimalRepository(AppDbContext context): IAdoptAnimalRepository
{
    public async Task<AdoptAnimal?> GetByIdAsync(int id)
    {
        return await context.AdoptAnimals.FirstOrDefaultAsync(a=>a.Id == id);
    }

    public async Task<IEnumerable<AdoptAnimal>> GetAllAsync()
    {
        return await context.AdoptAnimals
            .AsNoTracking() 
            .ToListAsync();
    }

    public async Task UpdateAdoptAnimal(AdoptAnimal adoptAnimal)
    {
        context.AdoptAnimals.Update(adoptAnimal);
        await context.SaveChangesAsync();
    }

    public async Task CreateAdoptAnimal(AdoptAnimal adoptAnimal)
    {
        context.AdoptAnimals.Add(adoptAnimal);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAdoptAnimal(int id)
    {
        AdoptAnimal? adoptAnimal = context.AdoptAnimals.FirstOrDefault(a => a.Id == id);

        if (adoptAnimal != null)
        {
            context.AdoptAnimals.Remove(adoptAnimal);
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
        return await context.AdoptAnimals
            .Include(a => a.Animal)
            .Where(aa => aa.OwnerId == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<AdoptAnimal>> GetByUserIdAsync(int ownerId)
    {
        var usersAnimal = await context.AdoptAnimals
            .Include(aa => aa.Animal)
            .Where(aa => aa.OwnerId == ownerId)
            .ToListAsync();
        return usersAnimal;
    }
}