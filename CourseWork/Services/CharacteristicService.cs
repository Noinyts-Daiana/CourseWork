using CourseWork.DTOs;
using CourseWork.Repositories;
using CourseWork.Mappers;

namespace CourseWork.Services;

public class CharacteristicService(ICharacteristicRepository characteristicRepository): ICharacteristicService
{

    public async Task<IEnumerable<CharacteristicDto>> GetCharacteristicsAsync()
    {
        var characteristics = await characteristicRepository.GetCharacteristicsAsync();
        return characteristics.Select(c=>c.ToDto());
    }

    public async Task<CharacteristicDto> GetCharacteristicAsync(int characteristicId)
    {
        var characteristic = await characteristicRepository.GetCharacteristicsByIdAsync(characteristicId);
        if (characteristic == null)
        {
            throw new ArgumentException($"Характеристику з ID {characteristicId} не знайдено.");
        }

        return characteristic.ToDto();
    }

    public async Task AddCharacteristicAsync(CharacteristicDto characteristic)
    {
        await characteristicRepository.AddCharacteristicAsync(characteristic.ToEntity());
    }

    public async Task UpdateCharacteristicAsync(CharacteristicDto characteristic)
    {
        await characteristicRepository.UpdateCharacteristicAsync(characteristic.ToEntity());
    }

    public async Task DeleteCharacteristicAsync(int characteristicId)
    {
        await characteristicRepository.DeleteCharacteristicAsync(characteristicId);
    }
}