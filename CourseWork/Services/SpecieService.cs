using CourseWork.DTOs;
using CourseWork.Repositories;
using CourseWork.Mappers;

namespace CourseWork.Services;

public class SpecieService(ISpecieRepository specieRepository) : ISpecieService
{
    public async Task<SpeciesDto?> GetSpecieAsync(int id)
    {
        var specieFromDB = await specieRepository.GetSpecieAsync(id);

        return specieFromDB?.ToDto();
    }
    public async Task<IEnumerable<SpeciesDto>> GetAllSpeciesAsync()
    {
        var speciesFromDB = await specieRepository.GetAllSpeciesAsync();
        
        var speciesDto = speciesFromDB.Select(s => s.ToDto());
        
        return speciesDto;
    }

    public async Task AddSpecieAsync(SpeciesDto species)
    {
        await specieRepository.AddSpecieAsync(species.ToEntity());
    }
    
    public async Task DeleteSpecieAsync(int id)
    {
        await specieRepository.DeleteSpecieAsync(id);
    }

    public async Task<bool> UpdateSpecieAsync(int id, SpeciesDto speciesDto)
    {
        var existingSpecie = await specieRepository.GetSpecieAsync(id);
        if (existingSpecie == null) return false; 
        
        existingSpecie.Name = speciesDto.Name;
        existingSpecie.Slug = speciesDto.Slug;

        await specieRepository.UpdateSpecieAsync(existingSpecie);
    
        return true; 
    }
}