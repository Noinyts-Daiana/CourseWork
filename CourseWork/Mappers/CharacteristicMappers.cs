using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class CharacteristicMappers
{
    public static Characteristic ToEntity(this CharacteristicDto characteristicDto)
    {
        return new Characteristic()
        {
            Name = characteristicDto.Name
        };
    }

    public static CharacteristicDto ToDto(this Characteristic characteristic)
    {
        return new CharacteristicDto()
        {
            Id = characteristic.Id,
            Name = characteristic.Name
        };
    }
}