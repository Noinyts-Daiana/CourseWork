using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class AnimalRepository(AppDbContext context): IAnimalRepository
{

    public async Task<IEnumerable<Animal>> GetAnimalsAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(term));
        }
        
        int skip = (pageNumber - 1) * pageSize;
        return await query
            .OrderByDescending(a => a.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Animal?> GetAnimalByIdAsync(int id)
    {
        return await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByNameAsync(string animalName)
    {
        return await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.Name.Contains(animalName)) 
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByBreedAsync(int breedId)
    {
        return await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.BreedId == breedId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetAnimalsBySpeciesAsync(int speciesId)
    {
        return await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.SpeciesId == speciesId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByGenderAsync(Sex sex)
    {
        return await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.Sex == sex)
            .ToListAsync(); 
    }


    public async Task<Animal> AddAnimalAsync(Animal animal)
    {
        context.Animal.Add(animal);
        await context.SaveChangesAsync();
        return animal;
    }

    public async Task UpdateAnimalAsync(Animal animal)
    {
        context.Animal.Update(animal);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAnimalAsync(int id)
    {
        var animal = await context.Animal.FindAsync(id); 
    
        if (animal != null)
        {
            context.Animal.Remove(animal);
            await context.SaveChangesAsync(); 
        }
    }

    public async Task<int> GetAnimalsCountAsync()
    {
        return await context.Animal.CountAsync();
    }
}