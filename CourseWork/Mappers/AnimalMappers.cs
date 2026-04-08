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
            BreedId = animalDto.BreedId,
            SpeciesId = animalDto.SpeciesId,
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
            Weight = animal.Weight,
            IsSterilized = animal.IsSterilized,
            Description = animal.Description,
            Height = animal.Height,
            Sex = animal.Sex
        };
    }
}