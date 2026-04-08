using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class MedicalExamMappers
{
    public static MedicalExam ToEntity(this MedicalExamDTO medicalExamDto)
    {
        return new MedicalExam()
        {
            ExamDate = DateTime.SpecifyKind(medicalExamDto.ExamDate, DateTimeKind.Utc),
            AnimalId = medicalExamDto.AnimalId,
            Temperature = medicalExamDto.Temperature,
            Notes = medicalExamDto.Notes,
            Weight = medicalExamDto.Weight
        };
    }

    public static MedicalExamDTO ToDto(this MedicalExam medicalExam)
    {
        return new MedicalExamDTO()
        {
            Id = medicalExam.Id,
            ExamDate = medicalExam.ExamDate,
            AnimalId = medicalExam.AnimalId,
            Temperature = medicalExam.Temperature,
            Notes = medicalExam.Notes,
            Weight = medicalExam.Weight,
            AnimalName = medicalExam.Animal?.Name
        };
    }
}