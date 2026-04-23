using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class AnimalMappers
{
    public static Animal ToEntity(this AnimalDto animalDto)
    {
        return new Animal
        {
            Name = animalDto.Name,
            Birthday = animalDto.Birthday,
            BreedId = animalDto.BreedId??0,
            SpeciesId = animalDto.SpeciesId??0,
            Sex = animalDto.Sex,
            Height = animalDto.Height,
            Weight = animalDto.Weight,
            IsSterilized = animalDto.IsSterilized,
            Description = animalDto.Description
        };
    }

    public static AnimalDto ToDto(this Animal animal)
    {
        return new AnimalDto()
        {
            Id = animal.Id,
            Name = animal.Name,
            Birthday = animal.Birthday,
            BreedId = animal.BreedId,
            SpeciesId = animal.SpeciesId,
            SpeciesName = animal.Specie?.Name,
            Weight = animal.Weight,
            IsSterilized = animal.IsSterilized,
            Description = animal.Description,
            Height = animal.Height,
            Sex = animal.Sex,
            BreedName = animal.Breed?.Name,
            CharacteristicIds = animal.AnimalCharacteristics?
                .Select(ac => ac.CharacteristicId)
                .ToList() ?? new List<int>(),

            Characteristics = animal.AnimalCharacteristics?
                .Where(ac => ac.Characteristic != null)
                .Select(ac => ac.Characteristic!.Name)
                .ToList() ?? new List<string>(),
            Photos = animal.Photos.Select(p => new AnimalPhotoDto {
            Id = p.Id,
            FileUrl = p.FilePath,
            IsMain = p.IsMain
        }).ToList()
        };
    }
}