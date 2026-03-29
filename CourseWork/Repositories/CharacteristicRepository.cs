using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class CharacteristicRepository(AppDbContext context): ICharacteristicRepository
{
    public async Task<IEnumerable<Characteristic>> GetCharacteristicsAsync()
    {
        return await context.Characteristics.ToListAsync();
    }

    public async Task<Characteristic?> GetCharacteristicsByIdAsync(int id)
    {
        return await context.Characteristics.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCharacteristicAsync(Characteristic characteristic)
    {
        context.Characteristics.Add(characteristic);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCharacteristicAsync(int characteristicId)
    {
        Characteristic? characteristic = context.Characteristics.FirstOrDefault(a => a.Id == characteristicId);

        if (characteristic != null)
        {
            context.Characteristics.Remove(characteristic);
            await context.SaveChangesAsync();
           
        }
    }

    public async Task UpdateCharacteristicAsync(Characteristic characteristic)
    {
        context.Characteristics.Update(characteristic);
        await context.SaveChangesAsync();
    }
}