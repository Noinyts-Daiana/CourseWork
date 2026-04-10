using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class VaccinationMappers
{
    public static Vaccination ToEntity(this VaccinationDto vaccinationDto)
    {
        return new Vaccination()
        {
            VaccineName = vaccinationDto.VaccineName,
            DateAdministered = DateTime.SpecifyKind(vaccinationDto.DateAdministered, DateTimeKind.Utc),
            NextDueDate = DateTime.SpecifyKind(vaccinationDto.NextDueDate, DateTimeKind.Utc),
            AnimalId = vaccinationDto.AnimalId

        };
    }

    public static VaccinationDto ToDto(this Vaccination vaccination)
    {
        return new VaccinationDto()
        {
            Id = vaccination.Id,
            VaccineName = vaccination.VaccineName,
            DateAdministered = vaccination.DateAdministered,
            NextDueDate = vaccination.NextDueDate,
            AnimalId = vaccination.AnimalId,
            AnimalName = vaccination.Animal?.Name
        };
    }
}