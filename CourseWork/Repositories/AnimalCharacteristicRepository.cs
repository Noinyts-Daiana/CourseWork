using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class AnimalCharacteristicRepository(AppDbContext context): IAnimalCharacteristicRepository
{
    public async Task AddAsync(AnimalCharacteristic animalCharacteristic)
    {
       context.AnimalCharacteristics.Add(animalCharacteristic);
       await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(AnimalCharacteristic animalCharacteristic)
    {
        context.AnimalCharacteristics.Remove(animalCharacteristic);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Characteristic>> GetCharacteristicsByAnimalIdAsync(int animalId)
    {
        var characteristic = await context.AnimalCharacteristics
            .Where(ac => ac.AnimalId == animalId)
            .Select(ac => ac.Characteristic)
            .ToListAsync();

        return characteristic;
    }

    public async Task<IEnumerable<Animal>> GetAnimalsByCharacteristicIdAsync(int characteristicId)
    {
        var animal = await context.AnimalCharacteristics
            .Where(ac => ac.CharacteristicId == characteristicId)
            .Select(ac => ac.Animal)
            .ToListAsync();
        
        return animal;
    }

    public async Task RemoveAllForAnimalAsync(int animalId)
    {
        await context.AnimalCharacteristics
            .Where(ac => ac.AnimalId == animalId)
            .ExecuteDeleteAsync();
    }
}