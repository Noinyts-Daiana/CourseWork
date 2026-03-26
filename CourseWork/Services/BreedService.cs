using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using Microsoft.EntityFrameworkCore;
using CourseWork.Repositories;

namespace CourseWork.Services;

public class BreedService(IBreedRepository breedRepository): IBreedService
{
   public async Task<int> CreateBreedAsync(BreedsDto breedDto)
   {
       var newBreed = breedDto.ToEntityFromCreateDto();
       await breedRepository.AddAsync(newBreed);

       return newBreed.Id;
   }

   public async Task<IEnumerable<BreedsDto>> GetAllBreedsAsync()
   {
       var breedsFromDb = await breedRepository.GetAllAsync();

       var breedsDto = breedsFromDb.Select(b => b.ToDto());
       
       return breedsDto;
   }

   public async Task<BreedsDto?> GetBreedByIdAsync(int id)
   {
       var breed = await breedRepository.GetByIdAsync(id);

       if (breed == null)
       {
           return null; 
       }

       var breedDto = breed.ToDto();

       return breedDto;
   }

   public async Task<bool> UpdateBreedAsync(int id, BreedsDto breedDto)
   {
       var existingBreed = await breedRepository.GetByIdAsync(id);
    
       if (existingBreed == null)
       {
           return false;
       }

       existingBreed.Name = breedDto.Name;
       existingBreed.SpecieId = breedDto.SpeciesId;

       await breedRepository.UpdateAsync(existingBreed);

       return true;
   }

   public async  Task<IEnumerable<BreedsDto>> GetBreedsByNameAsync(string name)
   {
       var breeds = await breedRepository.GetBreedsByNameAsync(name);
       
       return breeds.Select(b => b.ToDto());
   }
}
