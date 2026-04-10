using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class VaccinationRepository(AppDbContext context): IVaccinationRepository
{
    public async Task<IEnumerable<Vaccination>> GetAllVaccinationsAsync(int pageNumber, int pageSize,
        string? searchTerm)
    {
        var query = context.Vaccination.AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var searchPattern = $"%{searchTerm}%";
            return query.Include(m=>m.Animal)
                .Where(v=> 
                    EF.Functions.ILike(v.Animal.Name, searchPattern) || 
                    EF.Functions.ILike(v.VaccineName, searchPattern));
        };
        
        int skip = (pageNumber - 1) * pageSize;
        return await context.Vaccination.Include(m=>m.Animal).Skip(skip).Take(pageSize).ToListAsync();
    }

    public async Task<Vaccination?> GetVaccinationAsync(int id)
    {
        return await context.Vaccination.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task AddVaccinationAsync(Vaccination vaccination)
    {
        context.Vaccination.Add(vaccination);
        await context.SaveChangesAsync();
    }

    public async Task UpdateVaccinationAsync(int id, Vaccination vaccination)
    {
        vaccination.Id = id;
        context.Vaccination.Update(vaccination);
        await context.SaveChangesAsync();
    }

    public async Task DeleteVaccinationAsync(int id)
    {
        var vaccination = context.Vaccination.Find(id);
        if (vaccination != null)
        {
            context.Vaccination.Remove(vaccination);
            await context.SaveChangesAsync();
        }
        
    }
    
    public async Task<int> GetVaccinationsCountAsync()
    {
        return await context.Vaccination.CountAsync();
    }
}