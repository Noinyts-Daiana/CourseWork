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
        var allHistory = await context.AdoptAnimal
            .Include(a => a.Animal)
            .ThenInclude(an => an.Breed)
            .ToListAsync();

        var available = allHistory
            .GroupBy(aa => aa.AnimalId)
            .Select(g => g.OrderByDescending(x => x.Id).First())
            .Where(aa => aa.Status == AdoptionStatus.Returned || aa.OwnerId == null) 
            .ToList();

        return available;
    }

    public async Task<IEnumerable<AdoptAnimal>> GetByUserIdAsync(int ownerId)
    {
        var history = await context.AdoptAnimal
            .Include(aa => aa.Animal)
            .Where(aa => aa.OwnerId == ownerId)
            .ToListAsync();

        return history
            .GroupBy(aa => aa.AnimalId)
            .Select(g => g.OrderByDescending(x => x.Id).First()) 
            .ToList();
    }
    public async Task<bool> IsAnimalAlreadyAdoptedAsync(int animalId)
    {
        return await context.AdoptAnimal
            .AnyAsync(a => a.AnimalId == animalId && a.Status == AdoptionStatus.Adopted);
    }
}