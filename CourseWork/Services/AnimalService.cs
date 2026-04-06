using System.Reflection.PortableExecutable;
using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories;

namespace CourseWork.Services;

public class AnimalService(IAnimalRepository animalRepository, IAdoptAnimalService adoptionService): IAnimalService
{
    public async Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync()
    {
        var animals = await animalRepository.GetAnimalsAsync();
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
        var animal = new Animal() 
        {
            Name = animalDto.Name,
            SpeciesId = animalDto.SpeciesId, 
            BreedId = animalDto.BreedId,
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
        await animalRepository.UpdateAnimalAsync(animalDto.ToEntity());
    }

}