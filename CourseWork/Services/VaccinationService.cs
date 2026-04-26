using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Repositories.Interfaces;

namespace CourseWork.Services;

public class VaccinationService(IVaccinationRepository vaccinationRepository): IVaccinationService
{
    public async Task<IEnumerable<VaccinationDto>> GetAllVaccinationsAsync(int pageNumber, int pageSize,
        string? searchTerm)
    {
        var vacinations = await vaccinationRepository.GetAllVaccinationsAsync(pageNumber, pageSize, searchTerm);
        var vaccinationDtos = vacinations.Select(v => v.ToDto());
        return vaccinationDtos;
    }

    public async Task<VaccinationDto?> GetVaccinationAsync(int id)
    {
        var vaccination = await vaccinationRepository.GetVaccinationAsync(id);
        return vaccination?.ToDto();
    }

    public async Task AddVaccinationAsync(VaccinationDto vaccination)
    {
        await vaccinationRepository.AddVaccinationAsync(vaccination.ToEntity());
    }

    public async Task UpdateVaccinationAsync(int id, VaccinationDto vaccination)
    {
        await vaccinationRepository.UpdateVaccinationAsync(id, vaccination.ToEntity());
    }

    public async Task DeleteVaccinationAsync(int id)
    {
        await  vaccinationRepository.DeleteVaccinationAsync(id);
    }

    public Task<int> GetVaccinationsCountAsync(string? searchTerm = null)
    {
        return vaccinationRepository.GetVaccinationsCountAsync(searchTerm);
    }

}