using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class MedicalExamRepository(AppDbContext context): IMedicalExamRepository
{
    public async Task<IEnumerable<MedicalExam>> GetMedicalExamsAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = context.MedicalExam.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLowerCase = searchTerm.ToLower();
            
            query = query.Include(m=>m.Animal)
                .Where(m=>m.Animal.Name.ToLower().Contains(searchTermLowerCase));

            return query;
        }
        
        int skip = (pageNumber - 1) * pageSize;
        return await context.MedicalExam.Include(m=>m.Animal)
            .OrderByDescending(m => m.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<MedicalExam?> GetMedicalExamAsync(int id)
    {
        return await context.MedicalExam.Include(m=>m.Animal).FirstOrDefaultAsync(m=>m.Id == id);
    }

    public async Task<IEnumerable<MedicalExam>> GetExamsByAnimalIdAsync(int animalId, int pageNumber, int pageSize)
    {
        return await  context.MedicalExam.Include(m=>m.Animal)
            .Where(m=>m.AnimalId == animalId)
            .OrderBy(m=>m.ExamDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddMedicalExamAsync(MedicalExam medicalExam)
    {
        context.MedicalExam.Add(medicalExam);
        await context.SaveChangesAsync();
    }

    public async Task UpdateMedicalExamAsync(int id, MedicalExam medicalExam)
    {
        medicalExam.Id = id;
        context.MedicalExam.Update(medicalExam);
        await context.SaveChangesAsync();
    }

    public async Task DeleteMedicalExamAsync(MedicalExam medicalExam)
    {
        context.MedicalExam.Remove(medicalExam);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetMedicalExamsCountAsync()
    {
        return await context.MedicalExam.CountAsync();
    }
}