using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IMedicalExamRepository
{
    Task<IEnumerable<MedicalExam>> GetMedicalExamsAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<MedicalExam?> GetMedicalExamAsync(int id);
    Task<IEnumerable<MedicalExam>> GetExamsByAnimalIdAsync(int animalId, int pageNumber, int pageSize);
    Task AddMedicalExamAsync(MedicalExam medicalExam);
    Task UpdateMedicalExamAsync(int id, MedicalExam medicalExam);
    Task DeleteMedicalExamAsync(int id);
    Task<int> GetMedicalExamsCountAsync(string? searchTerm = null);
    Task<IEnumerable<(int AnimalId, string AnimalName, DateTime? LastExamDate)>> GetAnimalsWithoutRecentExamAsync(DateTime threshold);
}