using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class AnimalRepository(AppDbContext context): IAnimalRepository
{
public async Task<IEnumerable<Animal>> GetAnimalsAsync(int pageNumber, int pageSize, string? searchTerm, List<int>? charIds, int? speciesId, int? breedId, int? sex, bool? isAdopted)
{
    var query = context.Animal
        .Include(a => a.Specie)
        .Include(a => a.Breed)
        .Include(a => a.AnimalCharacteristics)
        .ThenInclude(ac => ac.Characteristic)
        .Include(a => a.Photos)
        .Include(a => a.AdoptAnimals)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        query = query.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
    }

    if (charIds != null && charIds.Any())
    {
        query = query.Where(a => a.AnimalCharacteristics.Any(ac => charIds.Contains(ac.CharacteristicId)));
    }

    if (speciesId.HasValue)
    {
        query = query.Where(a => a.SpeciesId == speciesId.Value);
    }

    if (breedId.HasValue)
    {
        query = query.Where(a => a.BreedId == breedId.Value);
    }

    if (sex.HasValue)
    {
        query = query.Where(a => a.Sex == (Models.Sex)sex.Value);
    }

    if (isAdopted.HasValue)
    {
        if (isAdopted.Value) 
        {
            query = query.Where(a => a.AdoptAnimals.Any() && 
                                     a.AdoptAnimals.OrderByDescending(aa => aa.AdoptDate ?? aa.ArrivalDate)
                                                   .FirstOrDefault().Status == AdoptionStatus.Adopted);
        }
        else 
        {
            query = query.Where(a => !a.AdoptAnimals.Any() || 
                                     a.AdoptAnimals.OrderByDescending(aa => aa.AdoptDate ?? aa.ArrivalDate)
                                                   .FirstOrDefault().Status != AdoptionStatus.Adopted);
        }
    }

    return await query
        .OrderByDescending(a => a.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}
    public async Task<Animal?> GetAnimalByIdAsync(int id)
    {
        return await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Include(a => a.AnimalCharacteristics)
            .Include(a => a.AdoptAnimals)
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

    public async Task<int> GetAnimalsCountAsync(string? searchTerm, List<int>? charIds, int? speciesId, int? breedId, int? sex, bool? isAdopted)
    {
        var query = context.Animal.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));

        if (charIds != null && charIds.Any())
            query = query.Where(a => a.AnimalCharacteristics.Any(ac => charIds.Contains(ac.CharacteristicId)));

        if (speciesId.HasValue)
            query = query.Where(a => a.SpeciesId == speciesId.Value);

        if (breedId.HasValue)
            query = query.Where(a => a.BreedId == breedId.Value);

        if (sex.HasValue)
            query = query.Where(a => (int)a.Sex == sex.Value); 
        
        if (isAdopted.HasValue)
        {
            if (isAdopted.Value) 
                query = query.Where(a => a.AdoptAnimals.Any() && a.AdoptAnimals.OrderByDescending(aa => aa.AdoptDate ?? aa.ArrivalDate).FirstOrDefault().Status == AdoptionStatus.Adopted);
            else 
                query = query.Where(a => !a.AdoptAnimals.Any() || a.AdoptAnimals.OrderByDescending(aa => aa.AdoptDate ?? aa.ArrivalDate).FirstOrDefault().Status != AdoptionStatus.Adopted);
        }

        return await query.CountAsync();
    }
    
   
}