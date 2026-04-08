using CourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class CharacteristicRepository(AppDbContext context): ICharacteristicRepository
{
    public async Task<IEnumerable<Characteristic>> GetCharacteristicsAsync()
    {
        return await context.Characteristic.ToListAsync();
    }

    public async Task<Characteristic?> GetCharacteristicsByIdAsync(int id)
    {
        return await context.Characteristic.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCharacteristicAsync(Characteristic characteristic)
    {
        context.Characteristic.Add(characteristic);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCharacteristicAsync(int characteristicId)
    {
        Characteristic? characteristic = context.Characteristic.FirstOrDefault(a => a.Id == characteristicId);

        if (characteristic != null)
        {
            context.Characteristic.Remove(characteristic);
            await context.SaveChangesAsync();
           
        }
    }

    public async Task UpdateCharacteristicAsync(Characteristic characteristic)
    {
        context.Characteristic.Update(characteristic);
        await context.SaveChangesAsync();
    }
}