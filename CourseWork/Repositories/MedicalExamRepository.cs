using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class MedicalExamRepository(AppDbContext context) : IMedicalExamRepository
{
    public async Task<IEnumerable<MedicalExam>> GetMedicalExamsAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var query = context.MedicalExam.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLowerCase = searchTerm.ToLower();

            query = query.Include(m => m.Animal)
                .Where(m => m.Animal.Name.ToLower().Contains(searchTermLowerCase));

            return query;
        }

        int skip = (pageNumber - 1) * pageSize;
        return await context.MedicalExam.Include(m => m.Animal)
            .OrderByDescending(m => m.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<MedicalExam?> GetMedicalExamAsync(int id)
    {
        return await context.MedicalExam.Include(m => m.Animal).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MedicalExam>> GetExamsByAnimalIdAsync(int animalId, int pageNumber, int pageSize)
    {
        return await context.MedicalExam.Include(m => m.Animal)
            .Where(m => m.AnimalId == animalId)
            .OrderBy(m => m.ExamDate)
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

    public async Task DeleteMedicalExamAsync(int id)
    {
        var medicalExam = await context.MedicalExam.FindAsync(id);

        if (medicalExam != null)
        {
            context.MedicalExam.Remove(medicalExam);
            await context.SaveChangesAsync();
        }
    }

    public async Task<int> GetMedicalExamsCountAsync(string? searchTerm = null)
    {
        var query = context.MedicalExam.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(e => e.Animal.Name.Contains(searchTerm));
        }

        return await query.CountAsync();
    }

    /// <summary>
    /// Повертає тварин (що перебувають у притулку), у яких не було медогляду
    /// після вказаної дати (<paramref name="threshold"/>) або взагалі.
    /// </summary>
    public async Task<IEnumerable<(int AnimalId, string AnimalName, DateTime? LastExamDate)>>
        GetAnimalsWithoutRecentExamAsync(DateTime threshold)
    {
        // Тварини, що зараз у притулку (не усиновлені)
        var animalsInShelter = context.Animal
            .Where(a => !context.AdoptAnimal.Any(aa =>
                aa.AnimalId == a.Id && aa.Status == AdoptionStatus.Adopted));

        // Лівостороннє об'єднання з найновішим оглядом для кожної тварини
        var result = await animalsInShelter
            .GroupJoin(
                context.MedicalExam,
                a => a.Id,
                m => m.AnimalId,
                (a, exams) => new { a.Id, a.Name, LastExam = exams.Max(e => (DateTime?)e.ExamDate) })
            .Where(x => x.LastExam == null || x.LastExam < threshold)
            .Select(x => new { x.Id, x.Name, x.LastExam })
            .ToListAsync();

        return result.Select(x => (x.Id, x.Name, x.LastExam));
    }
}