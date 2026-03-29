using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class AnimalRepository(AppDbContext context): IAnimalRepository
{

    public async Task<IEnumerable<Animal>> GetAnimalsAsync()
    {
        return await context.Animals
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .ToListAsync();
    }

    public async Task<Animal?> GetAnimalByIdAsync(int id)
    {
        return await context.Animals
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByNameAsync(string animalName)
    {
        return await context.Animals
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.Name.Contains(animalName)) 
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByBreedAsync(int breedId)
    {
        return await context.Animals
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.BreedId == breedId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetAnimalsBySpeciesAsync(int speciesId)
    {
        return await context.Animals
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.SpeciesId == speciesId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByGenderAsync(Sex sex)
    {
        return await context.Animals
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Where(a => a.Sex == sex)
            .ToListAsync(); 
    }


    public async Task<Animal> AddAnimalAsync(Animal animal)
    {
        context.Animals.Add(animal);
        await context.SaveChangesAsync();
        return animal;
    }

    public async Task UpdateAnimalAsync(Animal animal)
    {
        context.Animals.Update(animal);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAnimalAsync(int id)
    {
        var animal = await context.Animals.FindAsync(id); 
    
        if (animal != null)
        {
            context.Animals.Remove(animal);
            await context.SaveChangesAsync(); 
        }
    }
}