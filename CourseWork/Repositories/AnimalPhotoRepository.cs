using CourseWork.Models;
using CourseWork.Repositories.Interfaces; 
using Microsoft.EntityFrameworkCore; 

namespace CourseWork.Repositories; 

public class AnimalPhotoRepository(AppDbContext context) : IAnimalPhotoRepository
{
    public async Task<IEnumerable<AnimalPhoto>> GetByAnimalIdAsync(int animalId)
    {
        return await context.AnimalPhoto
            .Where(p => p.AnimalId == animalId)
            .OrderByDescending(p => p.IsMain) 
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<AnimalPhoto?> GetByIdAsync(int id)
    {
        return await context.AnimalPhoto.FindAsync(id);
    }

    public async Task AddAsync(AnimalPhoto photo)
    {
        await context.AnimalPhoto.AddAsync(photo);
    }

    public async Task DeleteAsync(AnimalPhoto photo)
    {
        context.AnimalPhoto.Remove(photo);
        await Task.CompletedTask;
    }

    public async Task ClearMainPhotoFlagAsync(int animalId)
    {
        var mainPhotos = await context.AnimalPhoto
            .Where(p => p.AnimalId == animalId && p.IsMain)
            .ToListAsync();

        foreach (var p in mainPhotos)
        {
            p.IsMain = false;
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}