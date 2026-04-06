using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class SpecieMappers
{
    public static SpeciesDto ToDto(this Specie specie)
    {
        return new SpeciesDto
        {
            Name = specie.Name,
            Slug = specie.Slug
        };
    }

    public static Specie ToEntity(this SpeciesDto speciesDto)
    {
        return new Specie()
        {
            Name = speciesDto.Name,
            Slug = speciesDto.Slug
        };
    }
}