using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories;

namespace CourseWork.Services;

public class AnimalService(
    IAnimalRepository animalRepository, 
    IAdoptAnimalService adoptionService,
    ISpecieRepository speciesRepository,
    IBreedRepository breedRepository,
    ICharacteristicRepository characteristicRepository 
) : IAnimalService
{
    public async Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync(int pageNumber, int pageSize, string? searchTerm, List<int>? charIds, int? speciesId, int? breedId, int? sex)
    {
        var animals = await animalRepository.GetAnimalsAsync(pageNumber, pageSize, searchTerm, charIds, speciesId, breedId, sex);
        return animals.Select(a => a.ToDto());
    }

    public async Task<AnimalDto?> GetAnimalByIdAsync(int animalId)
    {
        var animal = await animalRepository.GetAnimalByIdAsync(animalId);
        
        if (animal is not null) return animal.ToDto();
        throw new ArgumentException($"Тваринку за {animalId} айді не знайдено.");
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
            var newBreed = new Breed { Name = animalDto.BreedName, SpecieId = animalDto.SpeciesId!.Value };
            await breedRepository.AddAsync(newBreed); 
            animalDto.BreedId = newBreed.Id;
        }

        animalDto.CharacteristicIds ??= new List<int>();
        if (animalDto.NewCharacteristicNames != null && animalDto.NewCharacteristicNames.Any())
        {
            foreach (var newName in animalDto.NewCharacteristicNames)
            {
                var newChar = new Characteristic { Name = newName };
                await characteristicRepository.AddCharacteristicAsync(newChar);
                animalDto.CharacteristicIds.Add(newChar.Id);
            }
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

        if (animalDto.CharacteristicIds.Any())
        {
            foreach (var charId in animalDto.CharacteristicIds)
            {
                animal.AnimalCharacteristics.Add(new AnimalCharacteristic { CharacteristicId = charId });
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
            var newBreed = new Breed { Name = animalDto.BreedName, SpecieId = animalDto.SpeciesId!.Value };
            await breedRepository.AddAsync(newBreed);
            animalDto.BreedId = newBreed.Id;
        }

        var existingAnimal = await animalRepository.GetAnimalByIdAsync(animalDto.Id);
        if (existingAnimal == null) throw new ArgumentException("Тварину не знайдено");

        existingAnimal.Name = animalDto.Name;
        existingAnimal.SpeciesId = animalDto.SpeciesId ?? 0;
        existingAnimal.BreedId = animalDto.BreedId ?? 0;
        existingAnimal.Birthday = animalDto.Birthday;
        existingAnimal.Sex = animalDto.Sex;
        existingAnimal.Weight = animalDto.Weight;
        existingAnimal.Height = animalDto.Height;
        existingAnimal.IsSterilized = animalDto.IsSterilized;
        existingAnimal.Description = animalDto.Description;

        if (animalDto.NewCharacteristicNames != null && animalDto.NewCharacteristicNames.Any())
        {
            foreach (var newName in animalDto.NewCharacteristicNames)
            {
                existingAnimal.AnimalCharacteristics.Add(new AnimalCharacteristic 
                { 
                    AnimalId = existingAnimal.Id,
                    Characteristic = new Characteristic { Name = newName } 
                });
            }
        }

        animalDto.CharacteristicIds ??= new List<int>();

        var characteristicsToRemove = existingAnimal.AnimalCharacteristics
            .Where(ac => ac.CharacteristicId != 0 && !animalDto.CharacteristicIds.Contains(ac.CharacteristicId))
            .ToList();

        foreach (var itemToRemove in characteristicsToRemove)
        {
            existingAnimal.AnimalCharacteristics.Remove(itemToRemove);
        }

        var existingCharIdsInDb = existingAnimal.AnimalCharacteristics
            .Select(ac => ac.CharacteristicId)
            .ToList();

        var characteristicsToAdd = animalDto.CharacteristicIds
            .Where(id => !existingCharIdsInDb.Contains(id))
            .ToList();

        foreach (var idToAdd in characteristicsToAdd)
        {
            existingAnimal.AnimalCharacteristics.Add(new AnimalCharacteristic 
            { 
                AnimalId = existingAnimal.Id, 
                CharacteristicId = idToAdd 
            });
        }

        await animalRepository.UpdateAnimalAsync(existingAnimal);
    }

    public async Task<int> GetAnimalsCountAsync(string? searchTerm, List<int>? charIds, int? speciesId, int? breedId, int? sex)
    {
        return await animalRepository.GetAnimalsCountAsync(searchTerm, charIds, speciesId, breedId, sex);
    }
}