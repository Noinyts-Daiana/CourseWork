using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class AdoptAnimalMappers
{
    public static AdoptAnimal ToEntity(this AdoptAnimalDto adoptAnimalDto)
    {
        return new AdoptAnimal
        {
            AnimalId = adoptAnimalDto.AnimalId,
            OwnerId = adoptAnimalDto.OwnerId,
            ArrivalDate = adoptAnimalDto.ArrivalDate ?? DateTime.UtcNow,
            AdoptDate = adoptAnimalDto.AdoptDate,
        };
    }

    public static AdoptAnimalDto ToDto(this AdoptAnimal adoptAnimal)
    {
        return new AdoptAnimalDto()
        {
            AnimalId = adoptAnimal.AnimalId,
            OwnerId = adoptAnimal.OwnerId,
            ArrivalDate = adoptAnimal.ArrivalDate,
            AdoptDate = adoptAnimal.AdoptDate,
        };
    }
}

