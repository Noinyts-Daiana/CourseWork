using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Repositories.Interfaces;

namespace CourseWork.Services;

public class MedicalService(IMedicalExamRepository medicalExamRepository) : IMedicalExamService
{
    public async Task<IEnumerable<MedicalExamDTO>> GetMedicalExamsAsync(int pageNumber, int pageSize,
        string? searchTerm)
    {
        var medicalExams = await medicalExamRepository.GetMedicalExamsAsync(pageNumber, pageSize, searchTerm);
        var medicalExamsDto = medicalExams.Select(m => m.ToDto());
        return medicalExamsDto;
    }

    public async Task<MedicalExamDTO?> GetMedicalExamAsync(int id)
    {
        var medicalExam = await medicalExamRepository.GetMedicalExamAsync(id);
        var medicalExamDto = medicalExam?.ToDto();
        return medicalExamDto;
    }

    public async Task<IEnumerable<MedicalExamDTO>> GetExamsByAnimalIdAsync(int animalId, int pageNumber, int pageSize)
    {
        var medicalExam = await medicalExamRepository.GetExamsByAnimalIdAsync(animalId, pageNumber, pageSize);
        var medicalExamsDto = medicalExam.Select(m => m.ToDto());
        return medicalExamsDto;
    }

    public async Task AddMedicalExamAsync(MedicalExamDTO medicalExamDto)
    {
        await medicalExamRepository.AddMedicalExamAsync(medicalExamDto.ToEntity());


    }

    public async Task UpdateMedicalExamAsync(int id, MedicalExamDTO medicalExamDto)
    {
        await medicalExamRepository.UpdateMedicalExamAsync(id, medicalExamDto.ToEntity());
    }

    public async Task DeleteMedicalExamAsync(MedicalExamDTO medicalExamDto)
    {
        await medicalExamRepository.DeleteMedicalExamAsync(medicalExamDto.ToEntity());
    }

    public async Task<int> GetMedicalExamsCountAsync()
    {
        return await medicalExamRepository.GetMedicalExamsCountAsync();
    }
}