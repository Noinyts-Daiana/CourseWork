using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class CharacteristicMappers
{
    public static Characteristic ToEntityFromToDto(this CharacteristicDto characteristicDto)
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
            Name = characteristic.Name
        };
    }
}