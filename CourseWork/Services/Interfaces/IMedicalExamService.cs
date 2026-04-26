using CourseWork.DTOs;

namespace CourseWork.Services;

public interface IMedicalExamService
{
    Task<IEnumerable<MedicalExamDTO>> GetMedicalExamsAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<MedicalExamDTO?> GetMedicalExamAsync(int id);
    Task<IEnumerable<MedicalExamDTO>> GetExamsByAnimalIdAsync(int animalId, int pageNumber, int pageSize);
    Task AddMedicalExamAsync(MedicalExamDTO medicalExamDto);
    Task UpdateMedicalExamAsync(int id, MedicalExamDTO medicalExamDto);
    Task DeleteMedicalExamAsync(int id);
    Task<int> GetMedicalExamsCountAsync(string? searchTerm = null);
}