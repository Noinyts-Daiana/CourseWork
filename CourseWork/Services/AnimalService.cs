using System.Reflection.PortableExecutable;
using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories;

namespace CourseWork.Services;

public class AnimalService(
    IAnimalRepository animalRepository, 
    IAdoptAnimalService adoptionService,
    ISpecieRepository speciesRepository,
    IBreedRepository breedRepository      
) : IAnimalService
{
    public async Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        var animals = await animalRepository.GetAnimalsAsync(pageNumber, pageSize, searchTerm);
        return animals.Select(a => a.ToDto());
    }

    public async Task<AnimalDto?> GetAnimalByIdAsync(int animalId)
    {
        var animal = await animalRepository.GetAnimalByIdAsync(animalId);
        if (animal is not null)
        {
            return animal.ToDto();
        }
        else
        {
            throw new ArgumentException($"Тваринку за {animalId} айді не знайдено.");
        }
    }

    public async Task DeleteAnimalAsync(int animalId)
    {
        await animalRepository.DeleteAnimalAsync(animalId);
    }

    public async Task<AnimalDto> AddAnimalAsync(AnimalDto animalDto)
    {
        if (animalDto.SpeciesId == null && !string.IsNullOrWhiteSpace(animalDto.SpeciesName))
        {
            var newSpecies = new Specie { Name = animalDto.SpeciesName };
            
            await speciesRepository.AddSpecieAsync(newSpecies); 
            
            animalDto.SpeciesId = newSpecies.Id; 
        }

        if (animalDto.BreedId == null && !string.IsNullOrWhiteSpace(animalDto.BreedName))
        {
            var newBreed = new Breed 
            { 
                Name = animalDto.BreedName,
                SpecieId = animalDto.SpeciesId!.Value 
            };
            
            await breedRepository.AddAsync(newBreed); 
            
            animalDto.BreedId = newBreed.Id;
        }

        var animal = new Animal() 
        {
            Name = animalDto.Name,
            SpeciesId = animalDto.SpeciesId ?? 0, 
            BreedId = animalDto.BreedId ?? 0,
            Birthday = animalDto.Birthday,
            Sex = animalDto.Sex,
            Weight = animalDto.Weight,
            Height = animalDto.Height,
            IsSterilized = animalDto.IsSterilized,
            Description = animalDto.Description,
        
            AnimalCharacteristics = new List<AnimalCharacteristic>() 
        };

        if (animalDto.CharacteristicIds != null && animalDto.CharacteristicIds.Any())
        {
            foreach (var charId in animalDto.CharacteristicIds)
            {
                animal.AnimalCharacteristics.Add(new AnimalCharacteristic 
                { 
                    CharacteristicId = charId 
                });
            }
        }

        var savedAnimal = await animalRepository.AddAnimalAsync(animal);

        await adoptionService.RegisterArrivalAsync(savedAnimal.Id);

        animalDto.Id = savedAnimal.Id;
        return animalDto;
    }

    public async Task UpdateAnimalAsync(AnimalDto animalDto)
    {
        if (animalDto.SpeciesId == null && !string.IsNullOrWhiteSpace(animalDto.SpeciesName))
        {
            var newSpecies = new Specie { Name = animalDto.SpeciesName };
            await speciesRepository.AddSpecieAsync(newSpecies);
            animalDto.SpeciesId = newSpecies.Id;
        }
        if (animalDto.BreedId == null && !string.IsNullOrWhiteSpace(animalDto.BreedName))
        {
            var newBreed = new Breed 
            { 
                Name = animalDto.BreedName,
                SpecieId = animalDto.SpeciesId!.Value 
            };
            await breedRepository.AddAsync(newBreed);
            animalDto.BreedId = newBreed.Id;
        }

        var animalToUpdate = animalDto.ToEntity();
        
        animalToUpdate.Id = animalDto.Id;

        await animalRepository.UpdateAnimalAsync(animalToUpdate);
    }

    public async Task<int> GetAnimalsCountAsync()
    {
        return await animalRepository.GetAnimalsCountAsync();
    }
}