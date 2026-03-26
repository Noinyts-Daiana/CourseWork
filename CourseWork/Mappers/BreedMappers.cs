using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class BreedMappers
{
    public static BreedsDto ToDto(this Breed breedModel)
    {
        return new BreedsDto
        {
            Name = breedModel.Name,
            SpeciesId = breedModel.SpecieId
        };
    }
    
    public static Breed ToEntityFromCreateDto(this BreedsDto breedDto)
    {
        return new Breed
        {
            Name = breedDto.Name,
            SpecieId = breedDto.SpeciesId
        };
    }
}