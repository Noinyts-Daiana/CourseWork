using CourseWork.Models;

namespace CourseWork.Repositories.Interfaces;

public interface IMedicalExamRepository
{
    Task<IEnumerable<MedicalExam>> GetMedicalExamsAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<MedicalExam?> GetMedicalExamAsync(int id);
    Task<IEnumerable<MedicalExam>> GetExamsByAnimalIdAsync(int animalId, int pageNumber, int pageSize);
    Task AddMedicalExamAsync(MedicalExam medicalExam);
    Task UpdateMedicalExamAsync(int id, MedicalExam medicalExam);
    Task DeleteMedicalExamAsync(MedicalExam medicalExam);
    Task<int> GetMedicalExamsCountAsync();
}